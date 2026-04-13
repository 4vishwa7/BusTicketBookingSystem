using Microsoft.AspNetCore.Mvc;
using BusBookingSystem.API.Interfaces;
using BusBookingSystem.API.DTOs;

namespace BusBookingSystem.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-order/{bookingId}")]
        public async Task<IActionResult> CreateOrder(Guid bookingId)
        {
            try
            {
                var orderId = await _paymentService.CreateOrder(bookingId);
                return Ok(new { RazorpayOrderId = orderId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyPayment([FromBody] PaymentVerificationDto verification)
        {
            try
            {
                var isValid = await _paymentService.VerifyPayment(verification);
                if (isValid)
                {
                    return Ok(new { message = "Payment verified and booking confirmed." });
                }
                else
                {
                    return BadRequest(new { message = "Payment verification failed." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
