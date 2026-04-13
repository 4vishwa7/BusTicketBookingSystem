using System.ComponentModel.DataAnnotations;

namespace BusBookingSystem.API.Models
{
    public class BusRoute
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Source { get; set; }
        [Required]
        public string Destination { get; set; }
        public int DistanceKm { get; set; }
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
