using Microsoft.Extensions.Options;
using PosterBoi.Core.Configs;
using MailKit.Net.Smtp;
using MimeKit;
using PosterBoi.Core.Interfaces.Services;

namespace PosterBoi.Infrastructure.Services
{
    public class EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger) : IEmailService
    {
        private readonly EmailSettings _settings = settings.Value;
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
    }
}
