using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using PosterBoi.Core.Configs;
using PosterBoi.Core.Interfaces.Repositories;
using PosterBoi.Core.Interfaces.Services;
using PosterBoi.Infrastructure.Helpers;

namespace PosterBoi.Infrastructure.Services
{
    public class EmailService(IOptions<EmailSettings> settings, IUserRepository userRepository, ILogger<EmailService> logger) : IEmailService
    {
        private readonly EmailSettings _settings = settings.Value;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ILogger<EmailService> _logger = logger;

        public async Task<bool> SendEmailAsync(string to, string subject, string message)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_settings.From));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;

                email.Body = new TextPart("html")
                {
                    Text = message
                };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(
                    _settings.Host,
                    _settings.Port,
                    MailKit.Security.SecureSocketOptions.StartTls
                );

                await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email sending failed email confirmation");
                return false;
            }
        }
        public async Task<Result<bool>> ResendConfirmationEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return Result<bool>.Fail("The user does not exist");

            if (user.IsConfirmed)
                return Result<bool>.Fail("The user is already confirmed");

            user.Token = TokenGenerator.GenerateSecureToken(32);
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateUserAsync(user);

            var confirmationLink = $"https://posterboi.com/auth/confirm?token={user.Token}";
            var emailMessage = $"<p>Click here to confirm your account:</p> <a href='{confirmationLink}'>Confirm Email</a>";

            _ = SendEmailAsync(user.Email, Constanst.ConfirmEmailSubject, emailMessage);

            return Result<bool>.Ok(true);
        }
    }
}
