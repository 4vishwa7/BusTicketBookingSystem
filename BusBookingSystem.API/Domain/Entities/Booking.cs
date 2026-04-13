using BusBookingSystem.API.Domain.Enums;

namespace BusBookingSystem.API.Domain.Entities;

public class Booking
{
    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }
    public  required string PassengerName { get; set; }
    public  required int PassengerCount { get; set; }
    public string Phone { get; set; }
    public int BusId { get; set; }
    public  required DateTime TravelDate { get; set; }
    public int SeatNumber { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public BusType BusType { get; set; }
}
