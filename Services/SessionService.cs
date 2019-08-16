using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Valeo.Bot.Services.ValeoKeyboards;
using ValeoBot.Data.Entities;
using ValeoBot.Services.ValeoApi;

namespace ValeoBot.Services
{
    public class SessionService
    {
        private static List<User> _usersCache;
        private static List<Order> _ordersCache;
        private readonly ILogger<SessionService> logger;
        private readonly IValeoAPIService valeoApi;

        public SessionService(
            ILogger<SessionService> logger,
            IValeoAPIService valeoApi
            ) 
        {
            this.logger = logger;
            this.valeoApi = valeoApi;
        }

        public async Task AuthorizeUser(int chatId)
        {
            return;
        }

        public async Task UpdateOrder() 
        { 

        }
    }
}