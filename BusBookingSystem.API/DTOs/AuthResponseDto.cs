using BusBookingSystem.API.Models;

namespace BusBookingSystem.API.DTOs
{
    public class AuthResponseDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime? ExpiresAt { get; set; }
        public bool IsVerified { get; set; }
        public string? Message { get; set; }
    }
}
