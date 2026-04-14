namespace BusBookingSystem.API.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
        Task SendSmsAsync(string phoneNumber, string message);
    }
}
