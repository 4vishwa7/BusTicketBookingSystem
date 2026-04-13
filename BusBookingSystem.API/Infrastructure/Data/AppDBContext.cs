using BusBookingSystem.API.Data;
using BusBookingSystem.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusBookingSystem.API.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Booking> Bookings { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}