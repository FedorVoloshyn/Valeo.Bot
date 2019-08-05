using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ValeoBot.Configuration;

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

        public async Task<Doctor> GetDoctorsByCategory(string category)
        {

            string json = await _client.GetStringAsync(string.Format(_config.ValeoApi.DoctorsUrl, category))
                .ConfigureAwait(false);
            return JsonConvert.DeserializeObject<Doctor>(json);
        }

        private async Task<Time[]> GetFreeTimeByDoctor(Doctor doctor)
        {
            string json = await _client.GetStringAsync(string.Format(_config.ValeoApi.TimeUrl, doctor.Id))
                .ConfigureAwait(false);
            return JsonConvert.DeserializeObject<Time[]>(json);
        }
    }
}