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

namespace Valeo.Bot.Services
{

    public class AuthorizationService
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
                    "*Вітаємо! Теперь вам доступні функції боту. Для запису на прийом натисніть \"Записатись на прийом\".*",
                    parseMode : ParseMode.Markdown,
                    replyMarkup : ValeoKeyboardsService.DefaultKeyboard.Markup
                );
            }
            else
            {
                await valeoBot.Client.EditMessageTextAsync(
                    chatId,
                    lastReg.RegistrationMessageId.Value,
                    "*Вітаємо! Теперь вам доступні функції боту. Для запису на прийом натисніть \"Записатись на прийом\".*",
                    parseMode : ParseMode.Markdown,
                    replyMarkup : ValeoKeyboardsService.DefaultKeyboard.Markup
                );
                lastReg.RegistrationMessageId = null;
                regRepository.Update(lastReg);
            }
        }   
    }
}