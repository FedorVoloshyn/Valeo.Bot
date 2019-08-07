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
                    Message = "*Оберіть лікаря*",
                        Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
                            {
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Сім. лікар", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Підатр", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Терапевт", "PONG")
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Ендокринолог", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Кардіолог", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Гінеколог", "PONG"),
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
                    Message = "*УЗД (УЗІ)*",
                        Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
                            {
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Черевної порожнини", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Заочеревинного простору", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Щитовидної залози", "PONG")
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Молочної залози", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("М'яких тканин", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Лімфовузлів", "PONG"),
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Органів малого тазу", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Серця", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Судин ниж. кінцівок", "PONG")
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Судин верх. кінцівок", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Судин шиї та голови", "PONG"),
                                        InlineKeyboardButton.WithCallbackData("Нейросонографія", "PONG")
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