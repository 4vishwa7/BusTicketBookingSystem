using BusBookingSystem.API.Data;
using BusBookingSystem.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusBookingSystem.API.Services
{
    public class FareService : IFareService
    {
        private readonly AppDbContext _context;

        public FareService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> CalculateFare(Guid tripId, List<Guid> seatIds)
        {
            var trip = await _context.Trips.FindAsync(tripId);
            if (trip == null)
                throw new Exception("Trip not found.");

            var seats = await _context.Seats
                .Where(s => seatIds.Contains(s.Id))
                .ToListAsync();

            if (seats.Count != seatIds.Count)
                throw new Exception("One or more seats were not found.");

            decimal totalFare = 0;
            foreach (var seat in seats)
            {
                var seatFare = trip.BasePrice;

                // Example: Add surcharge for Sleeper seats
                if (seat.SeatType.Equals("Sleeper", StringComparison.OrdinalIgnoreCase))
                {
                    seatFare *= 1.5m;
                }

                totalFare += seatFare;
            }

            return totalFare;
        }
    }
}
