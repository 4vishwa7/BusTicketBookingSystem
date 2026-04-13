using BusBookingSystem.API.Domain.Enums;

namespace BusBookingSystem.API.API.Booking.DTO;

public class BookingResponseDTO
{
    public Guid BookingId { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}