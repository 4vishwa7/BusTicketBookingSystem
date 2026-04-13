using BusBookingSystem.API.Domain.Enums;

namespace BusBookingSystem.API.API.Booking.DTO;

public class BookingRequestDto
{
    public string PassengerName { get; set; }
    public int PassengerCount { get; set; }
    public string Phone { get; set; }
    public int BusId { get; set; }
    public BusType Bustype { get; set; }
    public DateTime TravelDate { get; set; }
    public int SeatNumber { get; set; }
}