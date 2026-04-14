namespace BusBookingSystem.API.Models
{
    public class Trip
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid BusId { get; set; }
        public Guid RouteId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal BasePrice { get; set; }
        public int AvailableSeats { get; set; }
        public Bus Bus { get; set; }
        public BusRoute Route { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
