using Telegram.Bot.Types.ReplyMarkups;
using System.Drawing;
using Telegram.Bot.Types;
using System.IO;
using System.Collections.Generic;

namespace Valeo.Bot.Services.ValeoKeyboards
{
    public struct ValeoKeyboard
    {
        public string Message { get; set; }
        public Location Location { get; set; }
        public InlineKeyboardMarkup Markup { get; set; }
        public string ImagePath { get; set; }
        public List<string> AlbumImagesPathList { get; set; }
    }

    public class Location
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}