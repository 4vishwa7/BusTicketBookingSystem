using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.API.Models
{
    public class Payment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Success, Failed, Refunded
        [Required]
        public string PaymentMethod { get; set; }
        [Required]
        public string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Booking Booking { get; set; }
    }
}
