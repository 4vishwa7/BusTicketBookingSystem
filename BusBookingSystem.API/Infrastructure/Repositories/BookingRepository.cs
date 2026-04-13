using BusBookingSystem.API.Data;
using BusBookingSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BusBookingSystem.API.Infrastructure.Repositories;

public class BookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
    }
    public async Task<List<Booking>> GetAllAsync()
    {
        return await _context.Bookings.ToListAsync();
    }
}