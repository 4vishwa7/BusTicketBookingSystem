using BusBookingSystem.API.API.Booking.DTO;
using BusBookingSystem.API.Application.Aggregator;
using BusBookingSystem.API.Application.Booking;
using BusBookingSystem.API.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BusBookingSystem.API.API.Booking;

[ApiController]
[Microsoft.AspNetCore.Components.Route("api/[controller]")]
public class BookingController: ControllerBase
{
    
    private readonly IBookingService _service;

    public BookingController(IBookingService service)
    {
        _service = service;
    }
    private readonly IAggregatorService _aggregator;

    public BookingController(IAggregatorService aggregator)
    {
        _aggregator = aggregator;
    }

    [HttpPost("book")]
    public async Task<IActionResult> Book(BookingRequestDto request)
    {
        var result = await _aggregator.BookTicketAsync(request);

        return Ok(new ApiResponse<BookingResponseDTO>(
            true,
            "Booking successful",
            result
        ));
    }
    
    [HttpPost("cancel/{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _service.CancelBookingAsync(id);
        return Ok(result);
    }
}