using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MimeKit;
using ValeoBot.Configuration;

namespace Valeo.Bot.Services
{
    public class MailingService : EmailSender
    {
        public MailingService(SMTPConnection settings) : base(settings)
        {
            this.settings = settings;
        }
        public override async Task SendEmailAsync(Feedback review)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(settings.CompanyName, settings.UserName));
            emailMessage.To.Add(new MailboxAddress("tikey.gm@gmail.com"));
            emailMessage.Subject = "Отзыв из чат-бота Валео Diagnostics";
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = review.Text
            };

            using(var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(settings.Server, settings.Port, settings.UseSSL);
                await client.AuthenticateAsync(settings.UserName, settings.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }

        }
    }
}