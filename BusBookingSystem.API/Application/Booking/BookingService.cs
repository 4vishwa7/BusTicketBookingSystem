using BusBookingSystem.API.API.Booking.DTO;
using BusBookingSystem.API.API.Hub;
using BusBookingSystem.API.Application.Booking.Validators;
using BusBookingSystem.API.Application.Pricing;
using BusBookingSystem.API.Domain.Enums;
using BusBookingSystem.API.Infrastructure.Repositories;
using BusBookingSystem.API.Shared.Exceptions;
using Microsoft.AspNetCore.SignalR;

namespace BusBookingSystem.API.Application.Booking;

public class BookingService
{
    private readonly IBookingRepository _repo;
    private readonly IPricingService _pricing;
    private readonly IBookingValidator _validator;
    
    private readonly IHubContext<SeatHub> _hub;

    public BookingService(
        IBookingRepository repo,
        IPricingService pricing,
        IBookingValidator validator,
        IHubContext<SeatHub> hub)
    {
        _repo = repo;
        _pricing = pricing;
        _validator = validator;
        _hub = hub;
    }
    

    public async Task<BookingResponseDTO> CreateBookingAsync(BookingRequestDto request)
    {
        
        var isAvailable = await _repo.IsSeatAvailable(request.BusId, request.SeatNumber);

        if (!isAvailable)
            throw new Exception("Seat already booked");

      
        var price = await _pricing.CalculatePrice(
            request.BusId,
            request.Bustype,
            request.SeatNumber,
            request.TravelDate
        );
        var booking = new Domain.Entities.Booking
        {
            UserId = request.UserId,
            PassengerName = request.PassengerName,
            Phone = request.Phone,
            BusId = request.BusId,
            BusType = request.Bustype,
            TravelDate = request.TravelDate,
            SeatNumber = request.SeatNumber,
            TotalPrice = price, 
            Status = BookingStatus.Confirmed,
            PassengerCount = request.PassengerCount
        };
        
        
        await _repo.AddAsync(booking);

        await _hub.Clients.All.SendAsync(
            "ReceiveSeatUpdate",
            booking.BusId,
            booking.SeatNumber
        );

        return new BookingResponseDTO
        {
            BookingId = booking.BookingId,
            TotalPrice = booking.TotalPrice,
            Status = booking.Status,
            CreatedAt = booking.CreatedAt
        };
    }
    public async Task<string> CancelBookingAsync(int bookingId)
    {
        var booking = await _repo.GetByIdAsync(bookingId);

        if (booking == null)
            throw new CustomException.NotFoundException("Booking not found");

        if (booking.Status == BookingStatus.Cancelled)
            throw new CustomException.BadRequestException("Booking already cancelled");

        if ((DateTime.UtcNow - booking.CreatedAt).TotalHours > 24)
            throw new Exception("Cancellation window expired");

        booking.Status = BookingStatus.Cancelled;

        await _repo.UpdateAsync(booking);

        return "Booking cancelled successfully";
    }
    
}