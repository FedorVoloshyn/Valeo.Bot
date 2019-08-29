using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ValeoBot.Configuration.Entities;
using ValeoBot.Services;

namespace Valeo.Bot.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class AuthorizationController
    {
        private readonly ILogger<AuthorizationController> logger;
        private readonly IOptions<BotConfig> botConfig;
        private readonly AuthorizationService authorizationService;

        public AuthorizationController(
            ILogger<AuthorizationController> logger,
            IOptions<BotConfig> botConfig,
            AuthorizationService authorizationService
        )
        {
            this.logger = logger;
            this.botConfig = botConfig;
            this.authorizationService = authorizationService;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Authorize(long id) 
        {
            await authorizationService.ApplyAuthorization(id);
            return new RedirectResult(string.Format("https://telegram.me/{0}", botConfig.Value.Username));
        }
    }
}