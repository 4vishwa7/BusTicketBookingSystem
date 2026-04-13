namespace BusBookingSystem.API.Application.Analytics;

public interface IAnalyticsService
{
    
    Task<int> GetTotalBookingsAsync();
    Task<decimal> GetTotalRevenueAsync();
}