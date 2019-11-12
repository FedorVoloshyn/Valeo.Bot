using System.Threading.Tasks;

namespace Valeo.Bot.Services
{
    public interface IMailingService
    {
         Task SendEmailAsync(Feedback review);
    }
}