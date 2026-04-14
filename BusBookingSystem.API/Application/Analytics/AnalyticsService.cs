using BusBookingSystem.API.Infrastructure.Repositories;

namespace BusBookingSystem.API.Application.Analytics;


using BusBookingSystem.API.Domain.Enums;

public class AnalyticsService : IAnalyticsService
{
    private readonly IBookingRepository _repo;

    public AnalyticsService(IBookingRepository repo)
    {
        _repo = repo;
    }

    public async Task<int> GetTotalBookingsAsync()
    {
        var bookings = await _repo.GetAllAsync();
        return bookings.Count();
    }

    public async Task<decimal> GetTotalRevenueAsync()
    {
        var bookings = await _repo.GetAllAsync();

        return bookings
            .Where(b => b.Status == BookingStatus.Confirmed)
            .Sum(b => b.TotalPrice);
    }
}
