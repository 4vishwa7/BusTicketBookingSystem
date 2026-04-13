using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.API.Models
{
    public class Bus
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string BusNumber { get; set; }
        public int TotalSeats { get; set; }
        [Required]
        public string BusType { get; set; } // AC / Sleeper
        [Required]
        public Guid OperatorId { get; set; }
        public BusOperator Operator { get; set; }
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
