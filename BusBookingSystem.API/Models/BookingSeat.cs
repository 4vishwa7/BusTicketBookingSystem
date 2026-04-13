using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.API.Models
{
    public class BookingSeat
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid BookingId { get; set; }
        [Required]
        public Guid SeatId { get; set; }
        public decimal Price { get; set; }
        public Booking Booking { get; set; }
        public Seat Seat { get; set; }
    }
}
