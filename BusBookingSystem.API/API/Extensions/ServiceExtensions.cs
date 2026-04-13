using BusBookingSystem.API.Application.Booking;
using BusBookingSystem.API.Application.Pricing;
using BusBookingSystem.API.Infrastructure.Repositories; 
using Microsoft.Extensions.DependencyInjection; 
using BusBookingSystem.API.Application.Booking.Validators;
using BusBookingSystem.API.Application.Aggregator;
using BusBookingSystem.API.Application.Analytics;
using BusBookingSystem.API.Application.Job;


namespace BusBookingSystem.API.API.Extensions;

public static class ServiceExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<BookingService>();
        services.AddScoped<BookingRepository>();
        services.AddScoped<IPricingService, PricingService>();
        services.AddScoped<IBookingValidator, BookingValidator>();
        services.AddScoped<IAggregatorService, AggregatorService>();
        services.AddScoped<IAnalyticsService, AnalyticsService>();
        services.AddScoped<BookingJob>();
    }
}