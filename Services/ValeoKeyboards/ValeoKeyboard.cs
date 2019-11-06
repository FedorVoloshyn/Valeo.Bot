using Telegram.Bot.Types.ReplyMarkups;

namespace Valeo.Bot.Services.ValeoKeyboards
{
    public struct ValeoKeyboard
    {
        public string Message { get; set; }
        public Location Location { get; set; }
        public InlineKeyboardMarkup Markup { get; set; }
    }

    public class Location
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }

}