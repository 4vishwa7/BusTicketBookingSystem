using BusBookingSystem.API.Data;
using BusBookingSystem.API.Interfaces;
using BusBookingSystem.API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BusBookingSystem.API.Services
{
    public class SeatService : ISeatService
    {
        private readonly AppDbContext _context;

        public SeatService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SeatStatusDto>> GetSeatsByTripId(Guid tripId)
        {
            var trip = await _context.Trips
                .Include(t => t.Bus)
                .FirstOrDefaultAsync(t => t.Id == tripId);

            if (trip == null)
                throw new Exception("Trip not found.");

            // Get all seats for the bus associated with this trip
            var allSeats = await _context.Seats
                .Where(s => s.BusId == trip.BusId)
                .ToListAsync();

            // Get IDs of seats already booked for this specific trip (and not cancelled)
            var bookedSeatIds = await _context.BookingSeats
                .Include(bs => bs.Booking)
                .Where(bs => bs.Booking.TripId == tripId && bs.Booking.Status != "Cancelled")
                .Select(bs => bs.SeatId)
                .ToListAsync();

            // Map to DTO with availability status
            var result = allSeats.Select(s => new SeatStatusDto
            {
                SeatId = s.Id,
                SeatNumber = s.SeatNumber,
                SeatType = s.SeatType,
                IsAvailable = !bookedSeatIds.Contains(s.Id)
            }).ToList();

            return result;
        }
    }
}
