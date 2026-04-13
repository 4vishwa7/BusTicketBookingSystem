namespace BusBookingSystem.API.Application.Analytics;


using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _service;

    public AnalyticsController(IAnalyticsService service)
    {
        _service = service;
    }

    [HttpGet("total-bookings")]
    public async Task<IActionResult> GetTotalBookings()
    {
        var result = await _service.GetTotalBookingsAsync();
        return Ok(result);
    }

    [HttpGet("total-revenue")]
    public async Task<IActionResult> GetTotalRevenue()
    {
        var result = await _service.GetTotalRevenueAsync();
        return Ok(result);
    }
}
