using BusBookingSystem.API.API.Booking.DTO;
using BusBookingSystem.API.Shared.Exceptions;

namespace BusBookingSystem.API.Application.Booking.Validators;


using BusBookingSystem.API.DTOs;
using Shared.Exceptions;

public class BookingValidator : IBookingValidator
{
    public void Validate(BookingRequestDto request)
    {
        // if (request.UserId <= 0)
        //     throw new CustomException.BadRequestException("Invalid UserId");

        if (string.IsNullOrWhiteSpace(request.PassengerName))
            throw new CustomException.BadRequestException("Passenger name is required");

        if (string.IsNullOrWhiteSpace(request.Phone))
            throw new CustomException.BadRequestException("Passenger phone  number is required");

        if (request.SeatNumber <= 0)
            throw new CustomException.BadRequestException("Invalid seat number");

        if (request.TravelDate < DateTime.UtcNow.Date)
            throw new CustomException.BadRequestException("Travel date cannot be in the past");
    }
}
