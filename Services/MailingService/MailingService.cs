using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using ValeoBot.Configuration;

namespace Valeo.Bot.Services
{
    public class MailingService : IMailingService
    {
        protected SMTPConnection _settings;
        private ILogger<MailingService> _logger;

        public MailingService(IOptions<SMTPConnection> settings, ILogger<MailingService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }
        public async Task SendEmailAsync(Feedback review)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_settings.CompanyName, _settings.UserName));
            emailMessage.To.Add(new MailboxAddress(_settings.Recipient));
            emailMessage.Subject = _settings.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = review.Text
            };

            try
            {
                using(var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    string logMessage = $"Started sending process. Settings:\nlogin:{_settings.UserName}\npassword:{_settings.Password}\nrecipient{_settings.Recipient}";
                    _logger.Log(LogLevel.Information, logMessage);
                    await client.ConnectAsync(_settings.Server, _settings.Port, _settings.UseSSL);
                    await client.AuthenticateAsync(_settings.UserName, _settings.Password);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                    _logger.Log(LogLevel.Information, $"Message '{emailMessage.Body}' sended to _settings.Recipient");
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error while trying to send email.");
            }
        }
    }
}