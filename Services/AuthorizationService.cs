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
    public class AuthorizationService : IAuthorization
    {
        private readonly ILogger<AuthorizationService> logger;
        private readonly IDataRepository<ValeoUser> userRepository;
        private readonly IDataRepository<Order> orderRepository;
        private readonly IDataRepository<Registration> regRepository;
        private readonly IValeoAPIService valeoApi;
        private readonly ValeoLifeBot valeoBot;
        private readonly IOptions<ValeoApiConfig> apiConfig;
        private readonly IOptions<BotConfig> botConfig;

        public AuthorizationService(
            ILogger<AuthorizationService> logger,
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
                    "*Вітаємо! Теперь вам доступні функції боту. Для запису на прийом натисніть \"Записатись до лікаря\".*",
                    parseMode : ParseMode.Markdown,
                    replyMarkup : ValeoKeyboardsService.DefaultKeyboard.Markup
                );
            }
            else
            {
                await valeoBot.Client.EditMessageTextAsync(
                    chatId,
                    lastReg.RegistrationMessageId.Value,
                    "*Вітаємо! Теперь вам доступні функції боту. Для запису на прийом натисніть \"Записатись до лікаря\".*",
                    parseMode : ParseMode.Markdown,
                    replyMarkup : ValeoKeyboardsService.DefaultKeyboard.Markup
                );
                lastReg.RegistrationMessageId = null;
                regRepository.Update(lastReg);
            }
        }   
        public bool IsAuthorized(Update update)
        {
            long chatId = 0;

            // TODO: all types cases
            switch(update.Type)
            {
                case UpdateType.CallbackQuery:
                    chatId = update.CallbackQuery.From.Id;
                    break;
                case UpdateType.Message:
                    chatId = update.Message.From.Id;
                    break;
                case UpdateType.InlineQuery:
                    chatId = update.InlineQuery.From.Id;
                    break;
                default:
                    chatId = 0;
                break;
            }

            return regRepository.Get(chatId).AuthServiceToken == null ? false : true;
        }
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
                "*Вітаємо. Для продовження роботи увійдіть до акаунту.*",
                parseMode : ParseMode.Markdown,
                replyMarkup : new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl("Війти", url))
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
    }
}