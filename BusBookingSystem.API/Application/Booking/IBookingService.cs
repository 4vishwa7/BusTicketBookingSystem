using BusBookingSystem.API.API.Booking.DTO;

namespace BusBookingSystem.API.Application.Booking;

public interface IBookingService
{
    Task<BookingResponseDTO> CreateBookingAsync(BookingRequestDto request);
    Task<string> CancelBookingAsync(int bookingId);
}