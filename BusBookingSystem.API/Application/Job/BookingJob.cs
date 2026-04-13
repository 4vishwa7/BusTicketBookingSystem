using BusBookingSystem.API.Infrastructure.Repositories;

namespace BusBookingSystem.API.Application.Job;
using BusBookingSystem.API.Domain.Enums;

public class BookingJob
{
    private readonly IBookingRepository _repo;

    public BookingJob(IBookingRepository repo)
    {
        _repo = repo;
    }

    public async Task CancelExpiredBookings()
    {
        var bookings = await _repo.GetAllAsync();

        foreach (var booking in bookings)
        {
            if (booking.Status == BookingStatus.Confirmed &&
                (DateTime.UtcNow - booking.CreatedAt).TotalHours > 24)
            {
                booking.Status = BookingStatus.Cancelled;
                await _repo.UpdateAsync(booking);
            }
        }
    }
}