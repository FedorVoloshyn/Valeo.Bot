using System;
using System.Collections.Generic;
using System.Globalization;
using Telegram.Bot.Types.ReplyMarkups;
using ValeoBot.Services.ValeoApi;
using ValeoBot.Services.ValeoApi.Models;

namespace Valeo.Bot.Services.ValeoKeyboards
{
    public class ValeoKeyboardsService
    {
        /** param1| Command, param2| Keyboard Markup */
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
        public static readonly ValeoKeyboard SuccessKeyboard = new ValeoKeyboard
        {
            Message = "Спасибо за обращение в Валео Diagnostics! В ближайшее время мы с Вами свяжемся.",
            Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
            new InlineKeyboardButton[]
            {
            InlineKeyboardButton.WithCallbackData("Записаться на прием", ValeoCommands.Doctors),
            }
            })
        };
        public static readonly ValeoKeyboard FailedKeyboard = new ValeoKeyboard
        {
            Message = "При обработку запроса произошла ошибка. Свяжитесь с администратором и попробоуйте повторить попытку позже.",
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
                                    InlineKeyboardButton.WithCallbackData("Семейный доктор", "semdoc|Doctors"),
                                        InlineKeyboardButton.WithCallbackData("Педиатр", "pediatr|Doctors"),
                                        InlineKeyboardButton.WithCallbackData("Терапевт", "terapevt|Doctors")
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Эндокринолог", "endocrin|Doctors"),
                                        InlineKeyboardButton.WithCallbackData("Кардиолог", "cardiol|Doctors"),
                                        InlineKeyboardButton.WithCallbackData("Гинеколог", "ginekol|Doctors"),
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Невропатолог", "nervopat|Doctors"),
                                        InlineKeyboardButton.WithCallbackData("Гастроэнтеролог", "gastroin|Doctors"),
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
                                    InlineKeyboardButton.WithCallbackData("Органы брюшной полости", "usibrush|Doctors"),
                                        InlineKeyboardButton.WithCallbackData("Органы забрюшинного пространства", "usizabrush|Doctors"),
                                        InlineKeyboardButton.WithCallbackData("Щитовидной железы", "usishitov|Doctors")
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Молочной железы", "usimoloch|Doctors"),
                                        InlineKeyboardButton.WithCallbackData("Мягких тканей", "usimyah|Doctors"),
                                        InlineKeyboardButton.WithCallbackData("Лимфоузлов", "usilimfous|Doctors"),
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Органов малого таза", "usitasa|Doctors"),
                                        InlineKeyboardButton.WithCallbackData("Сердца", "usiserdsa|Doctors"),
                                        InlineKeyboardButton.WithCallbackData("Сосудов нижних конечностей", "usisosudniz|Doctors")
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Сосудов верхних конечностей", "usisosudverh|Doctors"),
                                        InlineKeyboardButton.WithCallbackData("Сосудов шеи и головы", "usisosudshei|Doctors"),
                                        InlineKeyboardButton.WithCallbackData("Нейросонография", "usineyro|Doctors")
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

        public ValeoKeyboard CreateDoctorsKeyboard(List<Doctor> doctors)
        {

            List<InlineKeyboardButton[]> rows = new List<InlineKeyboardButton[]>();
            List<InlineKeyboardButton> currentRow = new List<InlineKeyboardButton>();
            for (int i = 1; i < doctors.Count; i++)
            {
                currentRow.Add(InlineKeyboardButton.WithCallbackData($"{doctors[i].FirstName} {doctors[i].LastName}", $"{doctors[i].FirstName}|Times"));
                if (i % 2 == 0)
                {
                    rows.Add(currentRow.ToArray());
                    currentRow.Clear();
                }
            }
            rows.Add(new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Назад", ValeoCommands.Doctors),
            });
            return new ValeoKeyboard() { Message = "Выберите доктора", Markup = new InlineKeyboardMarkup(rows) };
        }

        public ValeoKeyboard CreateTimesKeyboard(List<Time> times)
        {
            List<InlineKeyboardButton[]> rows = new List<InlineKeyboardButton[]>();
            List<InlineKeyboardButton> currentRow = new List<InlineKeyboardButton>();
            for (int i = 1; i < times.Count; i++)
            {
                string formatedTime = times[i].Value.ToString("g", CultureInfo.CreateSpecificCulture("es-ES"));
                currentRow.Add(InlineKeyboardButton.WithCallbackData($"{formatedTime}", $"{formatedTime}|Save"));
                if (i % 2 == 0)
                {
                    rows.Add(currentRow.ToArray());
                    currentRow.Clear();
                }
            }
            rows.Add(new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Назад", ValeoCommands.Doctors),
            });

            return new ValeoKeyboard() { Message = "Выберите время", Markup = new InlineKeyboardMarkup(rows) };
        }
    }
}