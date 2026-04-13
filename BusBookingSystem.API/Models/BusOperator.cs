using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.API.Models
{
    public class BusOperator
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; }
        [Required]
        public string ContactEmail { get; set; }
        [Required]
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Bus> Buses { get; set; } = new List<Bus>();
    }
}
