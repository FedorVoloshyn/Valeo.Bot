using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Valeo.Bot.Services.ValeoKeyboards;
using ValeoBot.Configuration.Entities;
using ValeoBot.Data.Entities;
using ValeoBot.Data.Repository;
using ValeoBot.Models;
using ValeoBot.Services.ValeoApi;

namespace ValeoBot.Services
{
    public class SessionService
    {
        #region Cahche
        private static readonly List<ValeoUser> _usersCache = new List<ValeoUser>();
        private static List<Order> _ordersCache = new List<Order>();
        #endregion

        private readonly ILogger<SessionService> logger;
        private readonly IDataRepository<ValeoUser> userRepository;
        private readonly IDataRepository<Order> orderRepository;
        private readonly IDataRepository<Registration> regRepository;
        private readonly IValeoAPIService valeoApi;
        private readonly ValeoLifeBot valeoBot;
        private readonly IOptions<ValeoApiConfig> apiConfig;
        private readonly IOptions<BotConfig> botConfig;

        public SessionService(
            ILogger<SessionService> logger,
            IDataRepository<ValeoUser> userRepository,
            IDataRepository<Order> orderRepository,
            IDataRepository<Registration> regRepository,
            ValeoLifeBot valeoBot,
            IValeoAPIService valeoApi,
            IOptions<ValeoApiConfig> apiConfig,
            IOptions<BotConfig> botConfig
        )
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.orderRepository = orderRepository;
            this.regRepository = regRepository;
            this.valeoBot = valeoBot;
            this.valeoApi = valeoApi;
            this.apiConfig = apiConfig;
            this.botConfig = botConfig;
        }

        public async Task ApplyAuthorization(long chatId)
        {
            var lastReg = regRepository.Get(chatId);
            if (lastReg == null || lastReg.RegistrationMessageId == null)
            {
                await valeoBot.Client.SendTextMessageAsync(
                    chatId,
                    "*Поздравляем! Теперь вам доступны функции бота. Для записи на прием нажмите \"Записаться на прием\".*",
                    parseMode : ParseMode.Markdown,
                    replyMarkup : ValeoKeyboardsService.DefaultKeyboard.Markup
                );
            }
            else
            {
                await valeoBot.Client.EditMessageTextAsync(
                    chatId,
                    lastReg.RegistrationMessageId.Value,
                    "*Поздравляем! Теперь вам доступны функции бота. Для записи на прием нажмите \"Записаться на прием\".*",
                    parseMode : ParseMode.Markdown,
                    replyMarkup : ValeoKeyboardsService.DefaultKeyboard.Markup
                );
                lastReg.RegistrationMessageId = null;
                regRepository.Update(lastReg);
            }

        }

        /**
          Example of google OAuth2: 
          https://accounts.google.com/o/oauth2/auth?access_type=offline&client_id=973674800009-gjgp1dsodh6qb3k89j31msosooq3ph2u.apps.googleusercontent.com&redirect_uri=https%3A%2F%2Fyt.contact.co%2Fauth%2Fyoutube&response_type=code&scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fyoutube.force-ssl&state=cbjhuyjq2kem4dar&approval_prompt=force
        */

        public async Task AuthorizeUser(Chat chat)
        {
            if (userRepository.Get(chat.Id) == null)
            {
                userRepository.Add(new ValeoUser() { Id = chat.Id, FirstName = chat.FirstName, LastName = chat.LastName, Nickname = chat.Username });
            }
            var url = string.Concat(
                apiConfig.Value.BaseUrl,
                string.Format(apiConfig.Value.AuthUrl, chat.Id, botConfig.Value.WebhookDomain));
            logger.LogInformation(url);
            var message = await valeoBot.Client.SendTextMessageAsync(
                chat.Id,
                "*Добро пожаловать. Для продолжения работы войдите в аккаунт.*",
                parseMode : ParseMode.Markdown,
                replyMarkup : new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl("Войти", url))
            );
            var lastReg = regRepository.Get(chat.Id);
            if (lastReg == null)
            {
                regRepository.Add(new Registration() { Id = chat.Id, Time = DateTime.Now, RegistrationMessageId = message.MessageId });
            }
            else
            {
                logger.LogInformation($"Override existing registration for {lastReg.Id}");
                lastReg.RegistrationMessageId = message.MessageId;
                lastReg.Time = DateTime.Now;
                regRepository.Update(lastReg);
            }

        }

        public async Task<bool> UpdateOrder(ValeoCommands command, long chatId)
        {
            try
            {
                switch (command.RequestType)
                {
                    case RequestType.Menu:
                        CleanOrder(chatId);
                        break;
                    case RequestType.Doctors:
                        AddCategory(chatId, command.Value);
                        break;
                    case RequestType.Times:
                        AddDoctor(chatId, command.Value);
                        break;
                    case RequestType.Save:
                        AddTime(chatId, command.Value);
                        break;
                    default:
                        logger.LogError($"Trying to create responce for unknown command: {command.OriginalValue}");
                        return false;
                }
            }
            catch (Exception e)
            {
                logger.LogError($"Failed to update order cache. command: {command.OriginalValue}; \n\r {e.ToString()}");
            }

            return true;
        }

        private void CleanOrder(long chatId)
        {
            var order = _ordersCache.FirstOrDefault(v => v.ChatId == chatId);
            if (order == null)
            {
                _ordersCache.Add(new Order() { ChatId = chatId });
            }
            else
            {
                order.Category = string.Empty;
                order.DoctorId = string.Empty;
                order.Time = string.Empty;
            }
        }

        private void AddCategory(long chatId, string value)
        {
            var order = _ordersCache.FirstOrDefault(v => v.ChatId == chatId);
            if (order == null)
            {
                _ordersCache.Add(new Order() { ChatId = chatId, Category = value });
            }
            else
            {
                order.Category = value;
                order.DoctorId = string.Empty;
                order.Time = string.Empty;
            }
        }
        private void AddDoctor(long chatId, string value)
        {
            var order = _ordersCache.FirstOrDefault(v => v.ChatId == chatId);
            if (order == null || string.IsNullOrEmpty(order.Category))
            {
                CleanOrder(chatId);
                throw new AggregateException($"Trying to add Doctor for order without category. Order: {order.ToString()}, chatId: {chatId}, value: {value}.");
            }
            else
            {
                order.DoctorId = value;
                order.Time = string.Empty;
            }
        }
        private void AddTime(long chatId, string value)
        {
            var order = _ordersCache.FirstOrDefault(v => v.ChatId == chatId);
            if (order == null || string.IsNullOrEmpty(order.Category) || string.IsNullOrEmpty(order.DoctorId))
            {
                CleanOrder(chatId);
                throw new AggregateException($"Trying to add Time for order without category or doctor. Order: {order.ToString()}, chatId: {chatId}, value: {value}.");
            }
            else
            {
                order.Time = value;
                orderRepository.Add(order);
            }
        }

        public async Task<bool> SaveOrder(long chatId)
        {
            var order = _ordersCache.FirstOrDefault(v => v.ChatId == chatId);
            if (order == null || string.IsNullOrEmpty(order.Category) || string.IsNullOrEmpty(order.DoctorId) || string.IsNullOrEmpty(order.Time))
            {
                CleanOrder(chatId);
                logger.LogError($"Trying to Save Order without important fields. Order: {order.ToString()}, chatId: {chatId}");
                return false;
            }

            if (!await valeoApi.SaveOrder(order))
            {
                logger.LogError($"Failed to save the order by api. Order: {order.ToString()}, chatId: {chatId}");
            }

            return true;
        }

    }
}