using BusBookingSystem.API.API.Booking.DTO;
using BusBookingSystem.API.Application.Booking;
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

    [HttpPost("book")]
    public async Task<IActionResult> Book(BookingRequestDto request)
    {
        var result = await _service.CreateBookingAsync(request);
        return Ok(result);
    }
    [HttpPost("cancel/{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _service.CancelBookingAsync(id);
        return Ok(result);
    }
}