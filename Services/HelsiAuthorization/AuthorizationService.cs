using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBWT.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Valeo.Bot.Configuration.Entities;
using Valeo.Bot.Data.Entities;
using Valeo.Bot.Data.Repository;
using Valeo.Bot.Services.HelsiAPI;
using Valeo.Bot.Services.ValeoKeyboards;

namespace Valeo.Bot.Services.HelsiAuthorization
{
    public class AuthorizationService : IAuthorization
    {
        private readonly ILogger<AuthorizationService> logger;
        private readonly IDataRepository<ValeoUser> userRepository;
        private readonly IDataRepository<Order> orderRepository;
        private readonly IDataRepository<Registration> regRepository;
        private readonly IHelsiAPIService HelsiAPI;
        private readonly TelegramBot valeoBot;
        private readonly IOptions<HelsiAPIConfig> apiConfig;
        private readonly IOptions<BotOptions> botConfig;

        public AuthorizationService(
            ILogger<AuthorizationService> logger,
            IDataRepository<ValeoUser> userRepository,
            IDataRepository<Order> orderRepository,
            IDataRepository<Registration> regRepository,
            TelegramBot valeoBot,
            IHelsiAPIService HelsiAPI,
            IOptions<HelsiAPIConfig> apiConfig,
            IOptions<BotOptions> botConfig
        )
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.orderRepository = orderRepository;
            this.regRepository = regRepository;
            this.valeoBot = valeoBot;
            this.HelsiAPI = HelsiAPI;
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
                    "*Вітаємо! Теперь вам доступні функції боту.*",
                    parseMode : ParseMode.Markdown,
                    replyMarkup : ValeoKeyboardsService.DefaultKeyboard.Markup
                );
            }
            else
            {
                await valeoBot.Client.EditMessageTextAsync(
                    chatId,
                    lastReg.RegistrationMessageId.Value,
                    "*Вітаємо! Теперь вам доступні функції боту.*",
                    parseMode : ParseMode.Markdown,
                    replyMarkup : ValeoKeyboardsService.DefaultKeyboard.Markup
                );
                lastReg.RegistrationMessageId = null;
                regRepository.Update(lastReg);
            }
        }
        public bool IsAuthorized(long chatId)
        {
            return regRepository.Get(chatId)?.AuthServiceToken == null ? false : true;
        }
        public async Task AuthorizeUser(Chat chat)
        {
            if (userRepository.Get(chat.Id) == null)
            {
                userRepository.Add(new ValeoUser() { Id = chat.Id, FirstName = chat.FirstName, LastName = chat.LastName, Nickname = chat.Username });
            }
            var url =
                string.Format(apiConfig.Value.Urls.Auth, chat.Id, botConfig.Value.WebhookDomain);
            logger.LogInformation(url);
            var message = await valeoBot.Client.SendTextMessageAsync(
                chat.Id,
                "*Вітаємо. Для продовження роботи увійдіть до акаунту.*",
                parseMode : ParseMode.Markdown,
                replyMarkup : new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl("Війти", url))
            );
            var lastReg = regRepository.Get(chat.Id);
            if (lastReg == null)
            {
                regRepository.Add(new Registration() { Id = chat.Id, Time = DateTime.Now, RegistrationMessageId = message.MessageId, AuthServiceToken = "test_token" });
            }
            else
            {
                logger.LogInformation($"Override existing registration for {lastReg.Id}");
                lastReg.RegistrationMessageId = message.MessageId;
                lastReg.Time = DateTime.Now;
                regRepository.Update(lastReg);
            }
        }
    }
}