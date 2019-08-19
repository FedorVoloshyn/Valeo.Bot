using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Policy;
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
        private static List<Data.Entities.ValeoUser> _usersCache;
        private static List<Order> _ordersCache;
        
        private readonly ILogger<SessionService> logger;
        private readonly IDataRepository<ValeoUser> userRepository;
        private readonly IDataRepository<Registration> regRepository;
        private readonly IValeoAPIService valeoApi;
        private readonly ValeoLifeBot valeoBot;
        private readonly IOptions<ValeoApiConfig> apiConfig;
        private readonly IOptions<BotConfig> botConfig;

        public SessionService(
            ILogger<SessionService> logger,
            IDataRepository<ValeoUser> userRepository,
            IDataRepository<Registration> regRepository,
            ValeoLifeBot valeoBot,
            IOptions<ValeoApiConfig> apiConfig,
            IOptions<BotConfig> botConfig
            ) 
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.regRepository = regRepository;
            this.valeoBot = valeoBot;
            this.apiConfig = apiConfig;
            this.botConfig = botConfig;
        }

        public async Task ApplyAuthorization(long chatId)
        {
            var lastReg = regRepository.Get(chatId);
            if (lastReg == null || lastReg.RegistrationMessageId == null) { 
                await valeoBot.Client.SendTextMessageAsync(
                    chatId,
                    "*Поздравляем! Теперь вам доступны функции бота. Для записи на прием нажмите \"Записаться на прием\".*",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: ValeoKeyboardsService.DefaultKeyboard.Markup
                );
            } else { 
                await valeoBot.Client.EditMessageTextAsync(
                    chatId,
                    lastReg.RegistrationMessageId.Value,
                    "*Поздравляем! Теперь вам доступны функции бота. Для записи на прием нажмите \"Записаться на прием\".*",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: ValeoKeyboardsService.DefaultKeyboard.Markup
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
            if(userRepository.Get(chat.Id) == null)
            { 
                userRepository.Add(new ValeoUser() { Id = chat.Id, FirstName = chat.FirstName, LastName = chat.LastName, Nickname = chat.Username });
            }
            var url = string.Concat(
                        apiConfig.Value.BaseUrl, 
                        string.Format(apiConfig.Value.AuthUrl, chat.Id, botConfig.Value.WebhookDomain
                        ));
            logger.LogInformation(url);
            var message = await valeoBot.Client.SendTextMessageAsync(
                chat.Id,
                "*Добро пожаловать. Для продолжения работы войдите в аккаунт.*",
                parseMode: ParseMode.Markdown,
                replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl("Войти", url))
            );
            var lastReg = regRepository.Get(chat.Id);
            if (lastReg == null) { 
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

        public async Task UpdateOrder() 
        { 

        }
    }
}