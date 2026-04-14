using Microsoft.AspNetCore.Mvc;
using BusBookingSystem.API.Interfaces;
using BusBookingSystem.API.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace BusBookingSystem.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var response = await _userService.Register(registerDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await _userService.Login(loginDto);
            if (response == null)
                return Unauthorized(new { message = "Invalid email or password." });

            return Ok(response);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto verifyOtpDto)
        {
            var result = await _userService.VerifyOtp(verifyOtpDto);
            if (result)
                return Ok(new { message = "Email verified successfully. You can now login." });
            
            return BadRequest(new { message = "Invalid or expired OTP." });
        }

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] string email)
        {
            var result = await _userService.ResendOtp(email);
            if (result)
                return Ok(new { message = "New OTP sent successfully." });

            return BadRequest(new { message = "User not found." });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProfile(Guid id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
                return NotFound();

            if (!user.IsVerified)
                return BadRequest(new { message = "Please verify your email first." });

            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.CreatedAt,
                user.IsVerified
            });
        }
    }
}
