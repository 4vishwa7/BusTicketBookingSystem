using BusBookingSystem.API.Domain.Enums;

namespace BusBookingSystem.API.Application.Pricing;

public class PricingService : IPricingService
{
    public Task<decimal> CalculatePrice(int busId, BusType busType, int seatNumber, DateTime travelDate)
    {
        decimal basePrice = 500;

        switch (busType)
        {
            case BusType.ACSleeper:
                basePrice += 300;
                break;

            case BusType.ACSeater:
                basePrice += 200;
                break;

            case BusType.NonACSleeper:
                basePrice += 150;
                break;

            case BusType.NonACSeater:
                basePrice += 0;
                break;
        }

        if (seatNumber >= 1 && seatNumber <= 5)
        {
            basePrice += 100;
        }

        if (seatNumber % 2 == 0)
        {
            basePrice += 50;
        }

        return Task.FromResult(basePrice);
    }
}