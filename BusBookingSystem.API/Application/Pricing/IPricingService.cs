using BusBookingSystem.API.Domain.Enums;

namespace BusBookingSystem.API.Application.Pricing;

public interface IPricingService
{
    Task<decimal> CalculatePrice(int busId, BusType busType, int seatNumber, DateTime travelDate);
}