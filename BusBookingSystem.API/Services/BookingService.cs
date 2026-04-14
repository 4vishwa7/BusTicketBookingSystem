using BusBookingSystem.API.Interfaces;
using BusBookingSystem.API.Models;
using BusBookingSystem.API.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace BusBookingSystem.API.Services
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;
        private readonly IDatabase _redis;

        public BookingService(AppDbContext context, IConnectionMultiplexer redis)
        {
            _context = context;
            _redis = redis.GetDatabase();
        }

        public async Task<Booking> CreateBooking(Guid userId, Guid tripId, List<Guid> seatIds, string? idempotencyKey = null)
        {
            // 1. Idempotency Check
            if (!string.IsNullOrEmpty(idempotencyKey))
            {
                var existingBooking = await _context.Bookings
                    .FirstOrDefaultAsync(b => b.UserId == userId && b.IdempotencyKey == idempotencyKey);
                
                if (existingBooking != null)
                    return existingBooking;
            }

            // 2. Redis Locking (5 minute seat lock)
            var lockKeys = seatIds.Select(s => $"seat_lock:{tripId}:{s}").ToList();
            bool allLocked = true;
            var acquiredLocks = new List<string>();

            try 
            {
                foreach (var lockKey in lockKeys)
                {
                    // Set key if not exists (NX) with expiry
                    bool locked = await _redis.StringSetAsync(lockKey, userId.ToString(), TimeSpan.FromMinutes(5), When.NotExists);
                    if (!locked)
                    {
                        allLocked = false;
                        break;
                    }
                    acquiredLocks.Add(lockKey);
                }

                if (!allLocked)
                    throw new Exception("One or more selected seats are temporarily locked by another user. Please try again in a few minutes.");

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var trip = await _context.Trips.Include(t => t.Bus).FirstOrDefaultAsync(t => t.Id == tripId);
                    if (trip == null)
                        throw new Exception("Trip not found.");

                    if (trip.AvailableSeats < seatIds.Count)
                        throw new Exception("Not enough available seats.");

                    // DB check for already booked seats
                    var alreadyBookedSeatIds = await _context.BookingSeats
                        .Where(bs => bs.TripId == tripId && bs.Booking.Status != "Cancelled")
                        .Select(bs => bs.SeatId)
                        .ToListAsync();

                    var unavailableSeats = seatIds.Intersect(alreadyBookedSeatIds).ToList();
                    if (unavailableSeats.Any())
                        throw new Exception($"Seats {string.Join(", ", unavailableSeats)} are already booked.");

                    var booking = new Booking
                    {
                        UserId = userId,
                        TripId = tripId,
                        Status = "Pending",
                        IdempotencyKey = idempotencyKey,
                        CreatedAt = DateTime.UtcNow,
                        TotalAmount = 0
                    };

                    _context.Bookings.Add(booking);

                    decimal totalAmount = 0;
                    foreach (var seatId in seatIds)
                    {
                        var price = trip.BasePrice;
                        totalAmount += price;

                        _context.BookingSeats.Add(new BookingSeat
                        {
                            BookingId = booking.Id,
                            SeatId = seatId,
                            TripId = tripId, // Necessary for unique constraint
                            Price = price
                        });
                    }

                    booking.TotalAmount = totalAmount;
                    trip.AvailableSeats -= seatIds.Count;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return booking;
                }
                catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true || ex.InnerException?.Message.Contains("Duplicate") == true)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Double booking detected. One of these seats was just booked by someone else.");
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            finally
            {
                // In a production app, you might NOT release the lock here if you want it to 
                // persist until payment is finished (Step 5). 
                // However, if the transaction succeeds, we can release it or let it expire.
                // For now, let's leave it locked for the full 5 minutes to protect the payment window.
            }
        }

        public async Task CancelBooking(Guid bookingId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var booking = await _context.Bookings
                    .Include(b => b.BookingSeats)
                    .FirstOrDefaultAsync(b => b.Id == bookingId);

                if (booking == null)
                    throw new Exception("Booking not found.");

                if (booking.Status == "Cancelled")
                    throw new Exception("Booking is already cancelled.");

                booking.Status = "Cancelled";

                var trip = await _context.Trips.FindAsync(booking.TripId);
                if (trip != null)
                {
                    trip.AvailableSeats += booking.BookingSeats.Count;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Booking?> GetBookingById(Guid bookingId)
        {
            return await _context.Bookings
                .Include(b => b.BookingSeats)
                .Include(b => b.Trip)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<List<Booking>> GetBookingsByUserId(Guid userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.BookingSeats)
                .Include(b => b.Trip)
                .ToListAsync();
        }
    }
}
