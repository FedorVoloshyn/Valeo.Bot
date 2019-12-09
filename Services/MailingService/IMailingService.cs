using System.Threading.Tasks;
using Valeo.Bot.Data.Entities;

namespace Valeo.Bot.Services
{
    public interface IMailingService
    {
         Task SendEmailAsync(Feedback review);
    }
}