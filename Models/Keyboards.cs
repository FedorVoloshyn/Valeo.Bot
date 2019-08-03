using Telegram.Bot.Types.ReplyMarkups;

namespace Valeo.Bot.Models
{
    public static class Keyboards
    {
        public static ReplyKeyboardMarkup WelcomeKeyboard 
        {
            get {
                return new ReplyKeyboardMarkup(new[]
                    {
                        new[] { new KeyboardButton("Записатись до лікаря") }
                    })
                    {
                        ResizeKeyboard = true
                    };
                }
        }

        public static ReplyKeyboardMarkup RejectKeyboard
        {
            get {
                return new ReplyKeyboardMarkup(new[]
                    {
                        new[] { new KeyboardButton("Відмінити запис") }
                    })
                    {
                        ResizeKeyboard = true
                    };
                }
        }

        public static InlineKeyboardMarkup DoctorList
        {
            get
            {
                return new InlineKeyboardMarkup(new[]
                {
                    new[] { new InlineKeyboardButton() }
                });
            }
        }
    }
}