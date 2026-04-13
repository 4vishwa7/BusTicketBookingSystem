using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.API.Models
{
    public class Cancellation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid BookingId { get; set; }
        public decimal RefundAmount { get; set; }
        public int RefundPercentage { get; set; }
        public DateTime CancelledAt { get; set; } = DateTime.UtcNow;
        [Required]
        public string Reason { get; set; }
        public Booking Booking { get; set; }
    }
}
