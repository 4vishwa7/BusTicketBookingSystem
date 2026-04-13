using BusBookingSystem.API.Domain.Entities;

namespace BusBookingSystem.API.Infrastructure.Repositories;

public interface IBookingRepository
{
    Task AddAsync(Booking booking);

    Task<Booking?> GetByIdAsync(int id);

    Task<bool> IsSeatAvailable(int busId, int seatNumber);

    Task UpdateAsync(Booking booking);
    
}