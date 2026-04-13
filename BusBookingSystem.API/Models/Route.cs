namespace BusBookingSystem.API.Models
{
    public class Route
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Source { get; set; }
        public string Destination { get; set; }
        public int DistanceKm { get; set; }
        public ICollection<Trip> Trips { get; set; } = new List<Trip>();
    }
}
