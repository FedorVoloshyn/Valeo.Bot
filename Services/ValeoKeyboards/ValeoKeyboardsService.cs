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
                InlineKeyboardButton.WithCallbackData("Записатись до лікаря", ValeoCommands.Doctors),
                InlineKeyboardButton.WithCallbackData("Контакти", ValeoCommands.Contacts),
            }
            })
        };
        public static readonly ValeoKeyboard SuccessKeyboard = new ValeoKeyboard
        {
            Message = "Дякуємо за звернення у Валео Diagnostics! Ми зв'яжемось з вами у найближчий час.",
            Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
            new InlineKeyboardButton[]
            {
            InlineKeyboardButton.WithCallbackData("Головне меню", ValeoCommands.Default),
            }
            })
        };
        public static readonly ValeoKeyboard FailedKeyboard = new ValeoKeyboard
        {
            Message = "При обробці запиту сталася помился. Зв'яжіться з адміністратором або спробуйте повторити спробу пізніше.",
            Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
            new InlineKeyboardButton[]
            {
            InlineKeyboardButton.WithCallbackData("Записатись до лікаря", ValeoCommands.Doctors),
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
                                    InlineKeyboardButton.WithCallbackData("Сімейний лікар", "semdoc|Doctors"),
                                    InlineKeyboardButton.WithCallbackData("Педіатр", "pediatr|Doctors")
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Терапевт", "terapevt|Doctors"),
                                    InlineKeyboardButton.WithCallbackData("Ендокринолог", "endocrin|Doctors")
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Кардіолог", "cardiol|Doctors"),
                                    InlineKeyboardButton.WithCallbackData("Гінеколог", "ginekol|Doctors")
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Невропатолог", "nervopat|Doctors"),
                                    InlineKeyboardButton.WithCallbackData("Гастроентеролог", "gastroin|Doctors")
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("УЗД", ValeoCommands.Usi)
                                },
                                new InlineKeyboardButton[]
                                {
                                    InlineKeyboardButton.WithCallbackData("Відміна", ValeoCommands.Default),
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
                                InlineKeyboardButton.WithCallbackData("Органи черевної порожнини", "usibrush|UsiInfo"),
                                InlineKeyboardButton.WithCallbackData("Органи заочеревинного простору", "usizabrush|UsiInfo")
                            },
                            new InlineKeyboardButton[]
                            {
                                InlineKeyboardButton.WithCallbackData("Щитовидної залози", "usishitov|UsiInfo"),
                                InlineKeyboardButton.WithCallbackData("Молочної залози", "usimoloch|UsiInfo")
                            },
                            new InlineKeyboardButton[]
                            {
                                InlineKeyboardButton.WithCallbackData("М'яких тканин", "usimyah|UsiInfo"),
                                InlineKeyboardButton.WithCallbackData("Лімфовузлів", "usilimfous|UsiInfo")
                            },                            
                            new InlineKeyboardButton[]
                            {
                                InlineKeyboardButton.WithCallbackData("Органів малого таза", "usitasa|UsiInfo"),
                                InlineKeyboardButton.WithCallbackData("Серця", "usiserdsa|UsiInfo")
                            },
                            new InlineKeyboardButton[]
                            {
                                InlineKeyboardButton.WithCallbackData("Судин нижніх кінцівок", "usisosudniz|UsiInfo"),
                                InlineKeyboardButton.WithCallbackData("Судин верхніх кінцівок", "usisosudverh|UsiInfo")
                            },
                            new InlineKeyboardButton[]
                            {
                                InlineKeyboardButton.WithCallbackData("Судин шиї та голови", "usisosudshei|UsiInfo"),
                                InlineKeyboardButton.WithCallbackData("Нейросонографія", "usineyro|UsiInfo")
                            },
                            new InlineKeyboardButton[]
                            {
                                InlineKeyboardButton.WithCallbackData("Назад", ValeoCommands.Doctors),
                            }
                        })
                });
            _keybords.Add(ValeoCommands.Contacts,
                new ValeoKeyboard
                {
                    Message = 
                        "__Контакти клініки *Valeo Diagnostic*__\n" + 
                        "\n*Сайт*: https://valeo.dp.ua/uk" + 
                        "\n*Телефон*: +38 (095) 232-34-00" + 
                        "\n*e-mail*: valeo.diagnostics@ukr.net",
                    Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>{
                        new InlineKeyboardButton[]
                        {
                            InlineKeyboardButton.WithCallbackData("Назад", ValeoCommands.Default),
                        }
                    })
                }
            );
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
            return new ValeoKeyboard() { Message = "Оберіть лікаря", Markup = new InlineKeyboardMarkup(rows) };
        }

        public ValeoKeyboard CreateTimesKeyboard(List<Time> times)
        {
            List<InlineKeyboardButton[]> rows = new List<InlineKeyboardButton[]>();
            List<InlineKeyboardButton> currentRow = new List<InlineKeyboardButton>();
            for (int i = 1; i < times.Count; i++)
            {
                string formatedTime = times[i].Value.ToString("g", CultureInfo.CreateSpecificCulture("es-ES")) 
                                    + " " 
                                    + times[i].Value.DayOfWeek.ToString();
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

            return new ValeoKeyboard() { Message = "Оберіть час", Markup = new InlineKeyboardMarkup(rows) };
        }
    
        public ValeoKeyboard CreateUziInfoKeyboard(string usiInfoId, string description)
        {
            var rows = new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Назад", ValeoCommands.Usi),
                InlineKeyboardButton.WithCallbackData("Далі", $"{usiInfoId}|Times")
            };

            return new ValeoKeyboard() { Message = description, Markup = new InlineKeyboardMarkup(rows) };
        }
    }
}