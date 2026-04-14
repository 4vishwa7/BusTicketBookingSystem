using BusBookingSystem.API.API.Booking.DTO;
using BusBookingSystem.API.Application.Booking;

namespace BusBookingSystem.API.Application.Aggregator;

using BusBookingSystem.API.DTOs;

public class AggregatorService : IAggregatorService
{
    private readonly IBookingService _bookingService;

    public AggregatorService(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<BookingResponseDTO> BookTicketAsync(BookingRequestDto request)
    {
        var booking = await _bookingService.CreateBookingAsync(request);

        return booking;
    }
}