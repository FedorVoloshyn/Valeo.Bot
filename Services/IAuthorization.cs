using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ValeoBot.Services
{
    public interface IAuthorization
    {
        Task ApplyAuthorization(long chatId);
        Task AuthorizeUser(Chat chat);
        bool IsAuthorized(Update chat);
    }
}