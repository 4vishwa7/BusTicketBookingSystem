using BusBookingSystem.API.DTOs;
using BusBookingSystem.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BusBookingSystem.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FareController : ControllerBase
    {
        private readonly IFareService _fareService;

        public FareController(IFareService fareService)
        {
            _fareService = fareService;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculateFare([FromBody] FareCalculationRequestDto request)
        {
            try
            {
                var fare = await _fareService.CalculateFare(request.TripId, request.SeatIds);
                return Ok(new { TotalFare = fare });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
