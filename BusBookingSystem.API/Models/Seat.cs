using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.API.Models
{
    public class Seat
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid BusId { get; set; }
        [Required]
        public string SeatNumber { get; set; }
        [Required]
        public string SeatType { get; set; } // Window / Sleeper
        public Bus Bus { get; set; }
    }
}
