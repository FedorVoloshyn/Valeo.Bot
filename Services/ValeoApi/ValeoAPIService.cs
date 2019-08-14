using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ValeoBot.Configuration;
using ValeoBot.Services.ValeoApi.Models;

namespace ValeoBot.Services.ValeoApi
{
    public class ValeoAPIService : IValeoAPIService
    {
        private readonly HttpClient _client;
        private ConfigProvider _config;

        public ValeoAPIService(ConfigProvider config)
        {
            _config = config;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_config.ValeoApi.BaseUrl)
            };
        }

        public async Task<List<Doctor>> GetDoctorsByCategory(string category)
        {

            string json = await _client.GetStringAsync(string.Format(_config.ValeoApi.DoctorsUrl, category))
                .ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<Doctor>>(json);
        }

        public async Task<List<Time>> GetFreeTimeByDoctor(Doctor doctor)
        {
            string json = await _client.GetStringAsync(string.Format(_config.ValeoApi.TimeUrl, doctor.Id))
                .ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<Time>>(json);
        }
    }
}