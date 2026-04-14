using BusBookingSystem.API.Models;
using BusBookingSystem.API.DTOs;

namespace BusBookingSystem.API.Interfaces
{
    public interface IUserService
    {
        Task<AuthResponseDto> Register(RegisterDto registerDto);
        Task<AuthResponseDto?> Login(LoginDto loginDto);
        Task<bool> VerifyOtp(VerifyOtpDto verifyOtpDto);
        Task<bool> ResendOtp(string email);
        Task<User?> GetById(Guid userId);
    }
}
