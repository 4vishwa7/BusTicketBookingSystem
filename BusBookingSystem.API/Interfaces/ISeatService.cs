using BusBookingSystem.API.DTOs;

namespace BusBookingSystem.API.Interfaces
{
    public interface ISeatService
    {
        Task<List<SeatStatusDto>> GetSeatsByTripId(Guid tripId);
    }
}
