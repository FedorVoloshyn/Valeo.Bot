using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Valeo.Bot.Services.ValeoKeyboards;
using ValeoBot.Services;

namespace ValeoBot.Models
{
    public class CallbackQueryHandler : IUpdateHandler
    {
        private readonly ResponseController _responseController;

        public CallbackQueryHandler(ResponseController responseController)
        {
            _responseController = responseController;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken)
        {
            CallbackQuery cq = context.Update.CallbackQuery;

            ValeoKeyboard reply = await _responseController.UpdateUserStateAsync(cq.Message.Chat.Id, cq.Data);

            if (reply.Location != null)
            {
                await context.Bot.Client.SendLocationAsync(
                    cq.Message.Chat.Id,
                    reply.Location.Latitude,
                    reply.Location.Longitude
                );
                await context.Bot.Client.SendTextMessageAsync(
                    cq.Message.Chat.Id,
                    reply.Message,
                    ParseMode.Markdown,
                    replyMarkup: reply.Markup
                );
                return;
            }

            if(reply.AlbumImagesPathList != null)
            {
                List<Stream> photosStreams = new List<Stream>();
                List<InputMediaPhoto> inputMediaPhotos = new List<InputMediaPhoto>();

                foreach(string path in reply.AlbumImagesPathList)
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
                    reply.Message,
                    replyMarkup : reply.Markup,
                    parseMode: ParseMode.Markdown
                );

                photosStreams.ForEach(stream => stream.Dispose());

                return;
            }

            if(reply.ImagePath != null)
            {
                await context.Bot.Client.DeleteMessageAsync(  
                    cq.Message.Chat.Id,
                    cq.Message.MessageId
                );
                using(var photo = new FileStream(reply.ImagePath, FileMode.Open))
                {
                    await context.Bot.Client.SendPhotoAsync(
                        cq.Message.Chat.Id,
                        photo,
                        caption: reply.Message,
                        replyMarkup: reply.Markup,
                        parseMode : ParseMode.Markdown
                    );
                }
                return;
            }

            if(cq.Message.Type == MessageType.Photo)
            {
                await context.Bot.Client.DeleteMessageAsync(  
                    cq.Message.Chat.Id,
                    cq.Message.MessageId
                );

                await context.Bot.Client.SendTextMessageAsync(
                    cq.Message.Chat.Id,
                    reply.Message,
                    replyMarkup : reply.Markup
                );
                return;
            }

            await context.Bot.Client.EditMessageTextAsync(
                cq.Message.Chat.Id,
                cq.Message.MessageId,
                reply.Message,
                ParseMode.Markdown);
            await context.Bot.Client.EditMessageReplyMarkupAsync(
                cq.Message.Chat.Id,
                cq.Message.MessageId,
                reply.Markup);

            await next(context);
        }
    }
}