namespace BusBookingSystem.API.Models
{
    public class Seat
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid BusId { get; set; }
        public string SeatNumber { get; set; }
        public string SeatType { get; set; } // Window / Sleeper
        public Bus Bus { get; set; }
    }
}
