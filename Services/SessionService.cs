using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBWT.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        #region Cache
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
        private readonly IOptions<BotOptions> botConfig;

        public SessionService(
            ILogger<SessionService> logger,
            IDataRepository<ValeoUser> userRepository,
            IDataRepository<Order> orderRepository,
            IDataRepository<Registration> regRepository,
            ValeoLifeBot valeoBot,
            IValeoAPIService valeoApi,
            IOptions<ValeoApiConfig> apiConfig,
            IOptions<BotOptions> botConfig
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

            bool isOrderSaved = await valeoApi.SaveOrder(order);
            if (!isOrderSaved)
            {
                logger.LogError($"Failed to save the order by api. Order: {order.ToString()}, chatId: {chatId}");
                return true;
            }

            string orderText = 
                "Дякуємо за звернення до клініки Valeo Diagnostic!\n\n" +
                "Ми записали вас на прийом.\n" + 
                "Ваш лікарь: " + $"{order.Category} {order.DoctorId}\n" + 
               $"Дата та час прийому: {order.Time}";
            await valeoBot.Client.SendTextMessageAsync(
                chatId,
                orderText
            );

            return true;
        }

    }
}