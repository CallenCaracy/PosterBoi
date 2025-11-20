namespace PosterBoi.Core.Interfaces.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string message);
    }
}
