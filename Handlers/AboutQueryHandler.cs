using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Valeo.Bot.Handlers
{
    public class AboutQueryHandler : IUpdateHandler
    {
        private const string Message = "Медичний центр *ВАЛЕО* працює з __26 вересня 2016 року__.\n\n" +
                      "Ми проводимо всі види *ультразвукових досліджень (УЗД)* на сучасному обладнанні і співпрацюємо з найнадійнішими лабораторіями в місті, що в комплексі дає високу точність постановки діагнозу і гарантію успішного лікування.\n\n" +
                      "Крім того, згідно з угодою з Національною Службою Здоров’я України на обслуговування пацієнтів за програмою медичних гарантій, усі зазначені нижче послуги (при укладенні договору з сімейним лікарем) в медичному центрі ВАЛЕО Ви отримуєте *безкоштовно*:\n" +
                      "⭕️ Прийом, консультація сімейного лікаря / терапевта / педіатра і його виклик додому (за необхідністю);\n" +
                      "⭕️ Загальний аналіз крові з лейкоцитарною формулою;\n" +
                      "⭕️ Загальний аналіз сечі;\n" +
                      "⭕️ Глюкоза крові;\n" +
                      "⭕️ Загальний холестерин;\n" +
                      "⭕️ Електрокардіограма;\n" +
                      "⭕️ Вимірювання артеріального тиску;\n" +
                      "⭕️ Експрес-тести на ВІЛ / геппатіт В, С / тропонін.";

        private static readonly ReadOnlyCollection<string> Images =
            new ReadOnlyCollection<string>(new[]
                {
                    "Resourses/Images/clinicPhoto/1.jpg",
                    "Resourses/Images/clinicPhoto/2.jpg",
                    "Resourses/Images/clinicPhoto/3.jpg",
                    "Resourses/Images/clinicPhoto/4.jpg",
                    "Resourses/Images/clinicPhoto/5.jpg",
                    "Resourses/Images/clinicPhoto/6.jpg",
                    "Resourses/Images/clinicPhoto/7.jpg",
                });
        private static readonly InlineKeyboardMarkup Markup =
            new InlineKeyboardMarkup(
                new InlineKeyboardButton[]
                {
                    InlineKeyboardButton.WithCallbackData("Головне меню ↩️", "back::"),
                });

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            List<InputMediaPhoto> inputMediaPhotos = new List<InputMediaPhoto>();

            foreach (string path in Images)
            {
                inputMediaPhotos.Add(new InputMediaPhoto(new InputMedia(System.IO.File.OpenRead(path), path)));
            }

            IAlbumInputMedia[] inputMedia = inputMediaPhotos.ToArray();

            await context.Bot.Client.DeleteMessageAsync(
                cq.Message.Chat.Id,
                cq.Message.MessageId
            );
            await context.Bot.Client.SendMediaGroupAsync(
               inputMedia,
               cq.Message.Chat.Id
            );
            await context.Bot.Client.SendTextMessageAsync(
                cq.Message.Chat.Id,
                Message,
                replyMarkup: Markup,
                parseMode: ParseMode.Markdown,
                cancellationToken: cancellationToken
            );

        }
    }
}