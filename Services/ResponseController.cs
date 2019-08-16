using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Valeo.Bot.Services.ValeoKeyboards;
using ValeoBot.Services.ValeoApi;

namespace ValeoBot.Services
{
    public class ResponseController
    {
        private readonly IValeoAPIService api;
        private readonly ValeoKeyboardsService _keyboardsService;

        public ResponseController(
            ApplicationDbContext context,
            IValeoAPIService api,
            ILogger<ResponseController> logger,
            ValeoKeyboardsService keyboardsService)
        {
            this.api = api;
            _keyboardsService = keyboardsService;
        }

        public ValeoKeyboard UpdateUserState(long chatId, ValeoCommands command)
        {
            if (command.RequestType == RequestType.Menu)
            {
                return _keyboardsService.GetKeyboard(command);
            }
            if (command.RequestType == RequestType.Doctors)
            {
                return CreateDoctors(command);
            }
            if (command.RequestType == RequestType.Times)
            {
                return CreateTimes(command);
            }

            return ValeoKeyboardsService.DefaultKeyboard;
        }

        private ValeoKeyboard CreateDoctors(string command)
        {
            var doctors = api.GetDoctorsByCategory(command).Result;
            return _keyboardsService.CreateDoctorsKeyboard(doctors);
        }

        private ValeoKeyboard CreateTimes(string command)
        {
            var times = api.GetFreeTimeByDoctor(command).Result;
            return _keyboardsService.CreateTimesKeyboard(times);
        }
    }
}