using BusBookingSystem.API.API.Booking.DTO;

namespace BusBookingSystem.API.Application.Aggregator;

public interface IAggregatorService
{
    Task<BookingResponseDTO> BookTicketAsync(BookingRequestDto request);
}