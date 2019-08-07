using Valeo.Bot.Services.ValeoKeyboards;

namespace ValeoBot.Services
{
    public class SessionService
    {
        private readonly ValeoKeyboardsService _keyboardsService;

        public SessionService(ValeoKeyboardsService keyboardsService)
        {
            _keyboardsService = keyboardsService;
        }

        public ValeoKeyboard UpdateUserState(long chatId, ValeoCommands command)
        {
            // TODO: Update order, then return next keyboard

            return _keyboardsService.GetKeyboard(command);
        }
    }
}