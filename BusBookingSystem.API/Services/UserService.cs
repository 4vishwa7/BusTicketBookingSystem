using BusBookingSystem.API.Interfaces;
using BusBookingSystem.API.Models;
using BusBookingSystem.API.Data;
using BusBookingSystem.API.DTOs;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace BusBookingSystem.API.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public UserService(AppDbContext context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<AuthResponseDto> Register(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                throw new Exception("Email already exists.");

            var otp = new Random().Next(100000, 999999).ToString();
            
            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                Phone = registerDto.Phone,
                PasswordHash = BC.HashPassword(registerDto.Password),
                OtpCode = otp,
                OtpExpiry = DateTime.UtcNow.AddMinutes(10),
                IsVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Send OTP via Email and SMS
            await _emailService.SendEmailAsync(user.Email, "Verify your Account", $"Your OTP is: {otp}");
            await _emailService.SendSmsAsync(user.Phone, $"Your Bus Booking OTP is: {otp}");

            return new AuthResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                IsVerified = false,
                Message = "Registration successful. Please verify your email with the OTP sent."
            };
        }

        public async Task<AuthResponseDto?> Login(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !BC.Verify(loginDto.Password, user.PasswordHash))
                return null;

            if (!user.IsVerified)
            {
                return new AuthResponseDto
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    IsVerified = false,
                    Message = "Account not verified. Please verify your OTP."
                };
            }

            var token = GenerateJwtToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Token = token.Token,
                ExpiresAt = token.ExpiresAt,
                IsVerified = true,
                Message = "Login successful."
            };
        }

        public async Task<bool> VerifyOtp(VerifyOtpDto verifyOtpDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == verifyOtpDto.Email);
            if (user == null) return false;

            if (user.OtpCode == verifyOtpDto.Otp && user.OtpExpiry > DateTime.UtcNow)
            {
                user.IsVerified = true;
                user.OtpCode = null;
                user.OtpExpiry = null;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> ResendOtp(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            var otp = new Random().Next(100000, 999999).ToString();
            user.OtpCode = otp;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(10);
            
            await _context.SaveChangesAsync();
            
            await _emailService.SendEmailAsync(user.Email, "New OTP", $"Your new OTP is: {otp}");
            await _emailService.SendSmsAsync(user.Phone, $"Your new Bus Booking OTP is: {otp}");

            return true;
        }

        public async Task<User?> GetById(Guid userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        private (string Token, DateTime ExpiresAt) GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"] ?? string.Empty);
            var expiresAt = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"] ?? "60"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = expiresAt,
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return (tokenHandler.WriteToken(token), expiresAt);
        }
    }
}
