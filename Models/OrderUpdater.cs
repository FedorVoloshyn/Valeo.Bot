using System.Threading;
using System.Threading.Tasks;
using ValeoBot.Data.Entities;
using ValeoBot.Data.Repository;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using User = ValeoBot.Data.Entities.User;
using Microsoft.Extensions.Logging;
using System;
using Valeo.Bot.Services.ValeoKeyboards;

namespace ValeoBot.Models
{
    public class OrderUpdater : IUpdateHandler
    {
        private ILogger<OrderUpdater> _logger;
        private IDataRepository<Order> _orderRepo;
        private IDataRepository<User> _userRepository;
        private string adminPassword = "Sladenkiy_Denis_Olegovich";

        public OrderUpdater(
            IDataRepository<Order> orderRepo,
            IDataRepository<User> userRepository,
            ILogger<OrderUpdater> logger)
        {
            _logger = logger;
            _orderRepo = orderRepo;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(IUpdateContext context, UpdateDelegate next, CancellationToken cancellationToken = default)
        {
            await ProcessRequest(context, cancellationToken);
        }

        public async Task ProcessRequest(IUpdateContext context, CancellationToken cancellationToken = default)
        {
            Message message = context.Update.Message;

            if (string.IsNullOrEmpty(message.Text))
                return;

            User profile = _userRepository.Get(message.Chat.Id);

            if (profile == null)
            {
                profile = _userRepository.Add(new User()
                {
                    Id = message.From.Id,
                    Nickname = message.From.Username
                });
            }

            _logger.LogInformation($"--- Handle request info: \n\rUser: {profile.ToString()}, \n\rMessage Text: {message.Text}");
            if (message.Text == adminPassword)
            {
                profile.IsAdmin = !profile.IsAdmin;
                _userRepository.Update(profile);
                if (profile.IsAdmin)
                    await context.Bot.Client.SendTextMessageAsync(message.Chat.Id, "Чат переведен в режим АДМИНИСТРАТОРА. Сюда будут поступать заявки!");
                else
                    await context.Bot.Client.SendTextMessageAsync(message.Chat.Id, "Чат ОТКЛЮЧЕН от режима АДМИНИСТРАТОРА!", replyMarkup: ValeoKeyboardsService.DefaultKeyboard.Markup);

                    return;
            }

            await context.Bot.Client.SendTextMessageAsync(message.Chat.Id, "Нажмите кнопку \"Заказать ремонт\".");
            return;
        }
    }
}