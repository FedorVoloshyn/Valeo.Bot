using System.Threading.Tasks;
using ValeoBot.Configuration;

namespace Valeo.Bot.Services
{
    public abstract class EmailSender
    {
        protected SMTPConnection settings;
        public EmailSender(SMTPConnection settings)
        {
            this.settings = settings;
        }
        public abstract Task SendEmailAsync(Feedback review);
    }
}