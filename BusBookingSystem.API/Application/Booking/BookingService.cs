using BusBookingSystem.API.API.Booking.DTO;
using BusBookingSystem.API.Application.Pricing;
using BusBookingSystem.API.Domain.Enums;
using BusBookingSystem.API.Infrastructure.Repositories;

namespace BusBookingSystem.API.Application.Booking;

public class BookingService
{
    private readonly IBookingRepository _repo;
    private readonly IPricingService _pricing;

    public BookingService(IBookingRepository repo, IPricingService pricing)
    {
        _repo = repo;
        _pricing = pricing;
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
            throw new Exception("Booking not found");

        if (booking.Status == BookingStatus.Cancelled)
            throw new Exception("Booking already cancelled");

        if ((DateTime.UtcNow - booking.CreatedAt).TotalHours > 24)
            throw new Exception("Cancellation window expired");

        booking.Status = BookingStatus.Cancelled;

        await _repo.UpdateAsync(booking);

        return "Booking cancelled successfully";
    }
    
}