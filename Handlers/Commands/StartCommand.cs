using System.Threading;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using Microsoft.Extensions.Logging;
using Valeo.Bot.Data.Entities;
using Valeo.Bot.Data.Repository;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Valeo.Bot.Services.ValeoKeyboards;

namespace Valeo.Bot.Handlers
{
    public class StartCommand : CommandBase
    {
        private readonly IDataRepository<ValeoUser> userRepository;
        private readonly ILogger<StartCommand> logger;

        public StartCommand(
            IDataRepository<ValeoUser> userRepository,
            ILogger<StartCommand> logger
        )
        {
            this.userRepository = userRepository;
            this.logger = logger;
        }
        public override async Task HandleAsync(
            IUpdateContext context,
            UpdateDelegate next,
            string[] args,
            CancellationToken cancellationToken
        )
        {
            var msg = context.Update.Message;
            if (userRepository.Get(msg.Chat.Id) == null)
            {
                logger.LogInformation($"User created {0}, {1}", msg.Chat.Id, msg.Chat.Username);
                userRepository.Add(new ValeoUser()
                {
                    Id = msg.Chat.Id,
                    FirstName = msg.Chat.FirstName,
                    LastName = msg.Chat.LastName,
                    Nickname = msg.Chat.Username
                });
            }   



            await context.Bot.Client.SendTextMessageAsync(
                msg.Chat,
                "Вітаю, користувач!👋 Буду радий допомогти тобі.\n Обери дiю з меню нижче 👇",
                ParseMode.Markdown,
                cancellationToken: cancellationToken
            );
            Thread.Sleep(1000);

            //await next(context, cancellationToken);
        }
    }
}