using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Valeo.Bot.Services.ValeoKeyboards;
using ValeoBot.Services.ValeoApi;

namespace ValeoBot.Services
{
    public class ResponseController
    {
        private readonly IValeoAPIService api;
        private readonly ILogger<ResponseController> logger;
        private readonly ValeoKeyboardsService _keyboardsService;
        private readonly SessionService sessionService;

        public ResponseController(
            IValeoAPIService api,
            ILogger<ResponseController> logger,
            ValeoKeyboardsService keyboardsService,
            SessionService sessionService)
        {
            this.api = api;
            this.logger = logger;
            _keyboardsService = keyboardsService;
            this.sessionService = sessionService;
        }

        public async Task<ValeoKeyboard> UpdateUserStateAsync(long chatId, ValeoCommands command)
        {
            bool updateResult = await sessionService.UpdateOrder(command, chatId);

            if (!updateResult)
            {
                FailedUpdate();
            }

            switch (command.RequestType)
            {
                case RequestType.Menu:
                    return _keyboardsService.GetKeyboard(command);
                case RequestType.Doctors:
                    return CreateDoctors(command);
                case RequestType.Times:
                    return CreateTimes(command);
                case RequestType.Save:
                    return await SaveOrder(chatId);
                default:
                    logger.LogError($"Trying to create responce for unknown command: {command.OriginalValue}");
                    return ValeoKeyboardsService.DefaultKeyboard;
            }
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

        private async Task<ValeoKeyboard> SaveOrder(long chatId)
        {
            bool result = await sessionService.SaveOrder(chatId);
            if (!result)
                return ValeoKeyboardsService.FailedKeyboard;

            return ValeoKeyboardsService.SuccessKeyboard;
        }
        private ValeoKeyboard FailedUpdate()
        {
            return ValeoKeyboardsService.FailedKeyboard;
        }
    }
}