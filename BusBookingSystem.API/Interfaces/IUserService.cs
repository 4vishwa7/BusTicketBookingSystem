using BusBookingSystem.API.Models;
using BusBookingSystem.API.DTOs;

namespace BusBookingSystem.API.Interfaces
{
    public interface IUserService
    {
        Task<User> Register(RegisterDto registerDto);
        Task<User?> Login(LoginDto loginDto);
        Task<User?> GetById(Guid userId);
    }
}
