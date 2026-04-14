using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.API.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public string? OtpCode { get; set; }
        public DateTime? OtpExpiry { get; set; }
        public bool IsVerified { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
