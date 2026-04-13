namespace BusBookingSystem.API.Interfaces;
using BusBookingSystem.API.Models;
public interface IBookingService
{
    Task<Booking> CreateBooking(Guid userId, Guid tripId, List<Guid> seatIds);
    Task CancelBooking (Guid bookingId);
    Task<Booking?> GetBookingById(Guid bookingId);
    Task<List<Booking>> GetBookingsByUserId(Guid userId);
}
