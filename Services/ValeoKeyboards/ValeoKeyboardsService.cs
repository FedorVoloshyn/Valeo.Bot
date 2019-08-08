using System;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Valeo.Bot.Services.ValeoKeyboards
{
    public class ValeoKeyboardsService
    {
        /** param1: Command, param2: Keyboard Markup */
        public static readonly ValeoKeyboard DefaultKeyboard = new ValeoKeyboard
        {
            Message = "Вітаємо у Valeo Diagnostic! Тут ви можете записатись на прийом до лікаря у нащій клінці. Натисніть *Записатись до лікаря* для оформлення заявки.",
            Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Записаться на прием", ValeoCommands.Doctors),
                }
            })
        };
        private static readonly Dictionary<ValeoCommands, ValeoKeyboard> _keybords = new Dictionary<ValeoCommands, ValeoKeyboard>();
        static ValeoKeyboardsService()
        {
            _keybords.Add(ValeoCommands.Default, DefaultKeyboard);
            _keybords.Add(ValeoCommands.Doctors,
                new ValeoKeyboard
                {
                    Message = "*Выберите доктора*",
                        Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
                            {
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Семейный доктор", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Педиатр", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Терапевт", "PONG")
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Эндокринолог", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Кардиолог", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Гинеколог", "PONG"),
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Невропатолог", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Гастроэнтеролог", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("УЗИ", ValeoCommands.Usi)
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Отмена", ValeoCommands.Default),
                                }
                            })
                });
            _keybords.Add(ValeoCommands.Usi,
                new ValeoKeyboard
                {
                    Message = "*УЗИ*",
                        Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
                            {
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Органы брюшной полости", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Органы забрюшинного пространства", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Щитовидной железы", "PONG")
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Молочной железы", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Мягких тканей", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Лимфоузлов", "PONG"),
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Органов малого таза", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Сердца", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Сосудов нижних конечностей", "PONG")
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Сосудов верхних конечностей", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Сосудов шеи и головы", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Нейросонография", "PONG")
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Назад", ValeoCommands.Doctors),
                                }
                            })
                });
        }

        public ValeoKeyboard GetKeyboard(ValeoCommands command)
        {
            return _keybords[command];
        }
    }
}