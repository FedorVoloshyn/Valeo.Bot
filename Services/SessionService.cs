using Microsoft.Extensions.Logging;
using Valeo.Bot.Services.ValeoKeyboards;

namespace ValeoBot.Services
{
    public class SessionService
    {
        private readonly ValeoKeyboardsService _keyboardsService;


        public SessionService(
            ApplicationDbContext context,
            ILogger<SessionService> logger,
            ValeoKeyboardsService keyboardsService)
        {
            _keyboardsService = keyboardsService;
        }

        public ValeoKeyboard UpdateUserState(long chatId, ValeoCommands command)
        {
            // TODO: Update order, then return next keyboard
            if (command == ValeoCommands.Default)
            {
                
            }
            return _keyboardsService.GetKeyboard(command);
        }
    }
}