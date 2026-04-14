using Microsoft.AspNetCore.Mvc;
using BusBookingSystem.API.Interfaces;

namespace BusBookingSystem.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpGet("trip/{tripId}")]
        public async Task<IActionResult> GetSeatsByTrip(Guid tripId)
        {
            try
            {
                var seats = await _seatService.GetSeatsByTripId(tripId);
                return Ok(seats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
