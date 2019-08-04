using System.Threading;
using System.Threading.Tasks;
using ValeoBot.Data.Entities;
using ValeoBot.Data.Repository;
using Telegram.Bot.Framework.Abstractions;
using Telegram.Bot.Types;
using User = ValeoBot.Data.Entities.User;
using Valeo.Bot.Models;
using Microsoft.Extensions.Logging;
using System;

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
                    await context.Bot.Client.SendTextMessageAsync(message.Chat.Id, "Чат ОТКЛЮЧЕН от режима АДМИНИСТРАТОРА!", replyMarkup: Keyboards.WelcomeKeyboard);

                    return;
            }

            if (message.Text == "Заказать ремонт")
            {
                Order newOrder;
                if(profile.LastOrderId == null)
                {
                    newOrder = _orderRepo.Add(new Order() { ChatId = message.Chat.Id });
                    profile.LastOrderId = newOrder.Id;
                    _userRepository.Update(profile);
                }

                await context.Bot.Client.SendTextMessageAsync(message.Chat.Id, "Введите свое имя", replyMarkup: Keyboards.RejectKeyboard);
                return;
            }

            if (message.Text == "Отменить заказ")
            {
                profile.LastOrderId = null;
                _userRepository.Update(profile);

                var orderToDelete = _orderRepo.Get(profile.LastOrderId.GetValueOrDefault());
                _orderRepo.Delete(orderToDelete);

                await context.Bot.Client.SendTextMessageAsync(message.Chat.Id, "Заказ отменен", replyMarkup: Keyboards.WelcomeKeyboard);
                return;
            }

            if (profile.LastOrderId != null)
            {
                // Order currentOrder = _orderRepo.Get(profile.LastOrderId.GetValueOrDefault());
                // currentOrder.ModTime = DateTime.Now;
                // _logger.LogInformation($"--- Handle request order:\n\rOrder: {currentOrder.ToString()}");
                
                // if (string.IsNullOrEmpty(currentOrder.Name))
                // {
                //     currentOrder.Name = message.Text;
                //     _orderRepo.Update(currentOrder);
                //     await context.Bot.Client.SendTextMessageAsync(message.Chat.Id, "Введите название вашего устройства");
                //     return;
                // }

                // if (string.IsNullOrEmpty(currentOrder.Device))
                // {
                //     currentOrder.Device = message.Text;
                //     _orderRepo.Update(currentOrder);
                //     await context.Bot.Client.SendTextMessageAsync(message.Chat.Id, "Опишите неполадку");
                //     return;
                // }

                // if (string.IsNullOrEmpty(currentOrder.Trouble))
                // {
                //     currentOrder.Trouble = message.Text;
                //     _orderRepo.Update(currentOrder);
                //     await context.Bot.Client.SendTextMessageAsync(message.Chat.Id, "Введите ваш номер телефона");
                //     return;
                // }

                // if (string.IsNullOrEmpty(currentOrder.PhoneNumber))
                // {
                //     currentOrder.PhoneNumber = message.Text;
                //     _orderRepo.Update(currentOrder);

                //     profile.LastOrderId = null;
                //     _userRepository.Update(profile);

                //     context.Bot.Client.SendTextMessageAsync(message.Chat.Id, "Заказ оформлен, мы свяжемся с вами в ближайшее время!", replyMarkup: Keyboards.WelcomeKeyboard);
                //     foreach (var admin in _userRepository.Find(e => e.Admin))
                //     {
                //         context.Bot.Client.SendTextMessageAsync(admin.Id, currentOrder.ToString());
                //     }
                //     return;
                // }
            }

            await context.Bot.Client.SendTextMessageAsync(message.Chat.Id, "Нажмите кнопку \"Заказать ремонт\".");
            return;
        }
    }
}