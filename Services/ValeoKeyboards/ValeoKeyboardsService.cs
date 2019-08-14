using System;
using System.Collections.Generic;
using System.Globalization;
using Telegram.Bot.Types.ReplyMarkups;
using ValeoBot.Services.ValeoApi;

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
        private static readonly Dictionary<ValeoCommands, ValeoKeyboard> _keybords = new Dictionary<ValeoCommands, ValeoKeyboard>();
        private readonly IValeoAPIService api;

        public ValeoKeyboardsService(IValeoAPIService api)
        {
            this.api = api;
        }
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
                                    InlineKeyboardButton.WithCallbackData("Семейный доктор", "semdoc|Doctors"),
                                        InlineKeyboardButton.WithCallbackData("Педиатр", "pediatr|Doctor"),
                                        InlineKeyboardButton.WithCallbackData("Терапевт", "terapevt|Doctor")
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Эндокринолог", "endocrin|Doctor"),
                                        InlineKeyboardButton.WithCallbackData("Кардиолог", "cardiol|Doctor"),
                                        InlineKeyboardButton.WithCallbackData("Гинеколог", "ginekol|Doctor"),
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Невропатолог", "nervopat|Doctor"),
                                        InlineKeyboardButton.WithCallbackData("Гастроэнтеролог", "gastroin|Doctor"),
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
                                    InlineKeyboardButton.WithCallbackData("Органы брюшной полости", "usibrush|Doctor"),
                                        InlineKeyboardButton.WithCallbackData("Органы забрюшинного пространства", "usizabrush|Doctor"),
                                        InlineKeyboardButton.WithCallbackData("Щитовидной железы", "usishitov|Doctor")
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Молочной железы", "usimoloch|Doctor"),
                                        InlineKeyboardButton.WithCallbackData("Мягких тканей", "usimyah|Doctor"),
                                        InlineKeyboardButton.WithCallbackData("Лимфоузлов", "usilimfous|Doctor"),
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Органов малого таза", "usitasa|Doctor"),
                                        InlineKeyboardButton.WithCallbackData("Сердца", "usiserdsa|Doctor"),
                                        InlineKeyboardButton.WithCallbackData("Сосудов нижних конечностей", "usisosudniz|Doctor")
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Сосудов верхних конечностей", "usisosudverh|Doctor"),
                                        InlineKeyboardButton.WithCallbackData("Сосудов шеи и головы", "usisosudshei|Doctor"),
                                        InlineKeyboardButton.WithCallbackData("Нейросонография", "usineyro|Doctor")
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
            if (command.RequestType == RequestType.Menu)
            {
                return _keybords[command];
            }
            if (command.RequestType == RequestType.Doctors)
            {
                return CreateDoctors(command);
            }
            if (command.RequestType == RequestType.Times)
            {
                return CreateTimes(command);
            }

            return DefaultKeyboard;
        }

        private ValeoKeyboard CreateDoctors(string command)
        {
            var doctors = api.GetDoctorsByCategory(command).Result;

            List<InlineKeyboardButton[]> rows = new List<InlineKeyboardButton[]>();
            List<InlineKeyboardButton> currentRow = new List<InlineKeyboardButton>();
            for (int i = 1; i <= doctors.Count; i++)
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

        private ValeoKeyboard CreateTimes(string command)
        {
            var times = api.GetFreeTimeByDoctor(command).Result;

            List<InlineKeyboardButton[]> rows = new List<InlineKeyboardButton[]>();
            List<InlineKeyboardButton> currentRow = new List<InlineKeyboardButton>();
            for (int i = 1; i < times.Count; i++)
            {
                string formatedTime = times[i].Value.ToString("g", CultureInfo.CreateSpecificCulture("es-ES"));
                currentRow.Add(InlineKeyboardButton.WithCallbackData($"{formatedTime}", $"{formatedTime}|Times"));
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