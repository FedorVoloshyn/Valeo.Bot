using Telegram.Bot.Types.ReplyMarkups;

namespace Valeo.Bot.Services.ValeoKeyboards
{
    public struct ValeoKeyboard
    {
        public string Message { get; set; }
        public InlineKeyboardMarkup Markup { get; set; }
    }

}