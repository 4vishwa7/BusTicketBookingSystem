using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.API.Models
{
    public class Booking
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid TripId { get; set; }
        public decimal TotalAmount { get; set; }
        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ConfirmedAt { get; set; }
        public User User { get; set; }
        public Trip Trip { get; set; }
        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
        public Payment Payment { get; set; }
    }
}
