using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Valeo.Bot.Services.HelsiAuthorization
{
    public interface IAuthorization
    {
        Task ApplyAuthorization(long chatId);
        Task AuthorizeUser(Chat chat);
        bool IsAuthorized(long chatId);
    }
}