using PosterBoi.Core.Configs;

namespace PosterBoi.Core.Interfaces.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string message);
        Task<Result<bool>> ResendConfirmationEmailAsync(string email);
    }
}
