using System.Collections.Generic;
using Valeo.Bot.Services.ValeoKeyboards;
using ValeoBot.Data.Entities;

namespace ValeoBot.Services
{
    public class SessionService
    {
        private readonly ValeoKeyboardsService _keyboardsService;
        private Dictionary<long, Order> _unterminatedOrders;

        public SessionService(ValeoKeyboardsService keyboardsService)
        {
            _keyboardsService = keyboardsService;
            _unterminatedOrders = new Dictionary<long, Order>();
        }

        public ValeoKeyboard UpdateUserState(long chatId, string command)
        {
            // TODO: Update order, then return next keyboard

            
            
            return _keyboardsService.GetKeyboard(command);
        }

        public ValeoKeyboard TerminateOrder(long chatId, string command)
        {
            return _keyboardsService.GetKeyboard("terminate");
        }
    }
}