using BusBookingSystem.API.DTOs;

namespace BusBookingSystem.API.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreateOrder(Guid bookingId);
        Task<bool> VerifyPayment(PaymentVerificationDto verification);
    }
}
