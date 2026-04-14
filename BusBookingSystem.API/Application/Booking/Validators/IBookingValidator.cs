using BusBookingSystem.API.API.Booking.DTO;

namespace BusBookingSystem.API.Application.Booking.Validators;

public interface IBookingValidator
{
    void Validate(BookingRequestDto request);
}