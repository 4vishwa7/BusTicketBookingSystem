namespace BusBookingSystem.API.DTOs
{
    public class SeatStatusDto
    {
        public Guid SeatId { get; set; }
        public string SeatNumber { get; set; }
        public string SeatType { get; set; }
        public bool IsAvailable { get; set; }
    }
}
