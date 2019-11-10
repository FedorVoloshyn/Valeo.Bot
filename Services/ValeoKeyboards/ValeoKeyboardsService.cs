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
            Message = "Вітаємо у Valeo Diagnostic! Тут ви можете оформити запис на прийом до лікаря у нашій клінці. Натисніть *Записатися на прийом* для оформлення заявки.",
            Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
                new InlineKeyboardButton[]
                {
                    //InlineKeyboardButton.WithCallbackData("Записатись до лікаря 💊", ValeoCommands.Doctors)
                    InlineKeyboardButton.WithUrl("Записатися на прийом 📝", "https://helsi.me/find-by-organization/2fd443d4-ffaa-493c-872c-5a9322c3237a")
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Наші лікарі 👨‍⚕️", ValeoCommands.OurDoctors),
                    InlineKeyboardButton.WithCallbackData("Залишити відгук ✍️", "feedback|Feedback")
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Адреси 📍", ValeoCommands.Location),
                    InlineKeyboardButton.WithCallbackData("Контакти 📞", ValeoCommands.Contacts)
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithSwitchInlineQuery("Поділитись 📢", "Звертайтесь до Валео Diagnostics! 🏥"),
                    InlineKeyboardButton.WithCallbackData("Про нас 🏥", ValeoCommands.About)
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
            InlineKeyboardButton.WithCallbackData("Головне меню ↩️", ValeoCommands.Default),
            },
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
        public static readonly ValeoKeyboard AboutKeyboard = new ValeoKeyboard
        {
            Message = $"Медичний центр *ВАЛЕО* працює з __26 вересня 2016 року__.\n\n" + 
                      $"Ми проводимо всі види *ультразвукових досліджень (УЗД)* на сучасному обладнанні і співпрацюємо з найнадійнішими лабораторіями в місті, що в комплексі дає високу точність постановки діагнозу і гарантію успішного лікування.\n\n" +
                      $"Крім того, згідно з угодою з Національною Службою Здоров’я України на обслуговування пацієнтів за програмою медичних гарантій, усі зазначені нижче послуги (при укладенні договору з сімейним лікарем) в медичному центрі ВАЛЕО Ви отримуєте *безкоштовно*:\n" +
                      $"⭕️ Прийом, консультація сімейного лікаря / терапевта / педіатра і його виклик додому (за необхідністю);\n" +
                      $"⭕️ Загальний аналіз крові з лейкоцитарною формулою;\n" +
                      $"⭕️ Загальний аналіз сечі;\n" +
                      $"⭕️ Глюкоза крові;\n" +
                      $"⭕️ Загальний холестерин;\n" +
                      $"⭕️ Електрокардіограма;\n" +
                      $"⭕️ Вимірювання артеріального тиску;\n" +
                      $"⭕️ Експрес-тести на ВІЛ / геппатіт В, С / тропонін."
            ,
            Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Головне меню ↩️", ValeoCommands.Default),
                }
            }),
            AlbumImagesPathList = new List<string>(
                new string[]
                {
                    "clinicPhoto/1.jpg",
                    "clinicPhoto/2.jpg",
                    "clinicPhoto/3.jpg",
                    "clinicPhoto/4.jpg",
                    "clinicPhoto/5.jpg",
                    "clinicPhoto/6.jpg",
                    "clinicPhoto/7.jpg",
                }
            )            
        };
        public static readonly ValeoKeyboard FeedbackKeyboard = new ValeoKeyboard
        {
            Message = $"Напишіть ваші враження та зауваження про якість обслуговування у клініці.\n",
            Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Головне меню ↩️", ValeoCommands.Default),
                }
            })
        };
        public static readonly ValeoKeyboard Locationskeyboard = new ValeoKeyboard
        {
            Message = $"*Днiпро*\nМедичний центр *ВАЛЕО*\nвулица Рабоча, 148, VIII-б",
            Location = new Location { Latitude = 48.4530569f, Longitude = 35.0029239f } ,
            Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Головне меню ↩️", ValeoCommands.Default),
                }
            })
        };

        public static readonly ValeoKeyboard DoctorsValeoStaticKeyboard = new ValeoKeyboard
        {
            Message = "Оберіть лікаря, до якого бажаєте записатись на прийом.",
            Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl("Сім. лікар Сафонов Д.О.", "https://helsi.me/doctor/9c2f65a7-4c36-49ae-864b-a51bbcfe52f0"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl("Терапевт Паливода Д.В.", "https://helsi.me/doctor/dd4d4f9c-0618-4d05-900d-627875bc7ddd"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl("Педіатр Макарченко К.В.", "https://helsi.me/doctor/dec67dba-fc15-4d2f-971d-4dff8a5e6120"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl("Терапевт Калита Н.В.", "https://helsi.me/doctor/757f686e-c28c-4ad5-acd1-71de4c3906d5"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithUrl("Терапевт Лєонова О.О.", "https://helsi.me/doctor/8db0a856-cb6e-480b-b9c8-37fbc6df9afe"),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Головне меню ↩️", ValeoCommands.Default),
                },
            })
        };
        public static readonly ValeoKeyboard OurDoctorsKeyboard = new ValeoKeyboard
        {
            Message = "Список спеціалістів клініки ВАЛЕО Diagnostics",
            Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>
            {
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Сафонов Денис Олегович", ValeoCommands.Safonov),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Паливода Дмитро Васильович", ValeoCommands.Palivoda),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Макарченко Катерина Вікторівна", ValeoCommands.Makarchenko),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Калита Наталя Вікторівна", ValeoCommands.Kalita),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Лєонова Оксана Олександрівна", ValeoCommands.Leonova),
                },
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Головне меню ↩️", ValeoCommands.Default),
                },
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
                        "__Контакти клініки *ВАЛЕО Diagnostics*__\n" + 
                        "\n*Сайт*: https://valeo.dp.ua/uk" + 
                        "\n*Телефон*: +380982323401" + 
                        "\n*Телефон*: +380952323400" + 
                        "\n*e-mail*: valeo.diagnostics@ukr.net",
                    Markup = new InlineKeyboardMarkup(new List<InlineKeyboardButton[]>{
                        new InlineKeyboardButton[]
                        {
                            InlineKeyboardButton.WithCallbackData("Головне меню ↩️", ValeoCommands.Default),
                        }
                    })
                }
            );
            _keybords.Add(ValeoCommands.About, AboutKeyboard);
            _keybords.Add(ValeoCommands.DoctorsStatic, DoctorsValeoStaticKeyboard);
            _keybords.Add(ValeoCommands.OurDoctors, OurDoctorsKeyboard);
            _keybords.Add(ValeoCommands.Feedback, FeedbackKeyboard);
            _keybords.Add(ValeoCommands.Location, Locationskeyboard);
            _keybords.Add(ValeoCommands.Safonov, ValeoDoctorsKeyboards.Safonov);
            _keybords.Add(ValeoCommands.Kalita, ValeoDoctorsKeyboards.Kalita);
            _keybords.Add(ValeoCommands.Makarchenko, ValeoDoctorsKeyboards.Makarchenko);
            _keybords.Add(ValeoCommands.Palivoda, ValeoDoctorsKeyboards.Palivoda);
            _keybords.Add(ValeoCommands.Leonova, ValeoDoctorsKeyboards.Leonova);
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