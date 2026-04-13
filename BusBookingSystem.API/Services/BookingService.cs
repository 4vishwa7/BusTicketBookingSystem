using BusBookingSystem.API.Interfaces;
using BusBookingSystem.API.Models;
using BusBookingSystem.API.Data;
using Microsoft.EntityFrameworkCore;

namespace BusBookingSystem.API.Services
{
    public class BookingService : IBookingService
    {
        private readonly AppDbContext _context;

        public BookingService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> CreateBooking(Guid userId, Guid tripId, List<Guid> seatIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var trip = await _context.Trips.FindAsync(tripId);
                if (trip == null)
                    throw new Exception("Trip not found.");

                if (trip.AvailableSeats < seatIds.Count)
                    throw new Exception("Not enough available seats.");

                // Check if any of the requested seats are already booked for this trip
                var alreadyBookedSeatIds = await _context.BookingSeats
                    .Include(bs => bs.Booking)
                    .Where(bs => bs.Booking.TripId == tripId && bs.Booking.Status != "Cancelled")
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
                    CreatedAt = DateTime.UtcNow,
                    TotalAmount = 0 // Will calculate below
                };

                _context.Bookings.Add(booking);

                decimal totalAmount = 0;
                foreach (var seatId in seatIds)
                {
                    // In a real system, price might be per trip/seat type. 
                    // For now, we use a simple logic or assume a fixed price from Trip.
                    var price = trip.BasePrice; 
                    totalAmount += price;

                    _context.BookingSeats.Add(new BookingSeat
                    {
                        BookingId = booking.Id,
                        SeatId = seatId,
                        Price = price
                    });
                }

                booking.TotalAmount = totalAmount;

                // Update available seats on the trip
                trip.AvailableSeats -= seatIds.Count;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return booking;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
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

                // Return seats to the trip
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
