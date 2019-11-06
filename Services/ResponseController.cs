using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Valeo.Bot.Services.ValeoKeyboards;
using ValeoBot.Services.ValeoApi;
using Valeo.Bot.Data.Repository;
using Valeo.Bot.Services.ReviewCashService;

namespace ValeoBot.Services
{
    public class ResponseController
    {
        private readonly IValeoAPIService _api;
        private readonly ILogger<ResponseController> _logger;
        private readonly ValeoKeyboardsService _keyboardsService;
        private readonly SessionService _sessionService;
        private readonly IReviewCacheService _reviewCacheService;

        public ResponseController(
            IValeoAPIService api,
            ILogger<ResponseController> logger,
            ValeoKeyboardsService keyboardsService,
            SessionService sessionService,
            IReviewCacheService reviewCacheService)
        {
            _api = api;
            _logger = logger;
            _keyboardsService = keyboardsService;
            _sessionService = sessionService;
            _reviewCacheService = reviewCacheService;
        }

        public async Task<ValeoKeyboard> UpdateUserStateAsync(long chatId, ValeoCommands command)
        {
            bool updateResult = await _sessionService.UpdateOrder(command, chatId);

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
                case RequestType.UsiInfo:
                    return CreateUziInfo(command);
                case RequestType.Save:
                    return await SaveOrder(chatId);
                case RequestType.Feedback:
                    return NewReview(chatId, command);
                case RequestType.Location:
                    return _keyboardsService.GetKeyboard(command);
                default:
                    _logger.LogError($"Trying to create responce for unknown command: {command.OriginalValue}");
                    return ValeoKeyboardsService.DefaultKeyboard;
            }
        }

        private ValeoKeyboard NewReview(long chatId, ValeoCommands command)
        {
            _reviewCacheService.AddReview(chatId);
            return _keyboardsService.GetKeyboard(command);
        }

        private ValeoKeyboard CreateDoctors(string command)
        {
            var doctors = _api.GetDoctorsByCategory(command).Result;
            //return _keyboardsService.CreateDoctorsKeyboard(doctors);
            return _keyboardsService.GetKeyboard(ValeoCommands.DoctorsStatic);
        }

        private ValeoKeyboard CreateTimes(string command)
        {
            var times = _api.GetFreeTimeByDoctor(command).Result;
            return _keyboardsService.CreateTimesKeyboard(times);
        }

        private ValeoKeyboard CreateUziInfo(string command)
        {
            string info = UziInfo.GetDescription(command);
            return _keyboardsService.CreateUziInfoKeyboard(command, info);
        }

        private async Task<ValeoKeyboard> SaveOrder(long chatId)
        {
            bool result = await _sessionService.SaveOrder(chatId);
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