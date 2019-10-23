using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ValeoBot.Configuration;
using ValeoBot.Configuration.Entities;
using ValeoBot.Data.Entities;
using ValeoBot.Services.ValeoApi.Models;

namespace ValeoBot.Services.ValeoApi
{
    public class ValeoAPIService : IValeoAPIService
    {
        private const int SafeExpirationGap = 60 * 5;
        private static AuthorizationData authData;

        public static AuthorizationData Auth
        {
            get
            {
                if (authData == null || authData.expiration_date < DateTime.Now)
                {
                    LoadAuth();
                }
                return authData;
            }
        }
        private HttpClient _client;
        private HttpClient Client
        {
            get
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Auth.token_type, Auth.access_token);
                return _client;
            }
        }

        private static void LoadAuth()
        {
            HttpClientHandler hmh = new HttpClientHandler()
            {
                // Proxy = new WebProxy("http://proxy.isd.dp.ua:8080")
            };
            HttpClient client = new HttpClient(hmh);
            string token = Startup.StaticConfiguration
                .GetSection("ValeoApi")
                .GetSection("Urls")
                .GetSection("Token").Value;
            ValeoApiAuth valeoAuth = Startup.StaticConfiguration
                .GetSection("ValeoApi")
                .GetSection("ValeoApiAuth").Get<ValeoApiAuth>();
            var responce = client.PostAsync(token, new FormUrlEncodedContent(
                new Dictionary<string, string>
                { { nameof(valeoAuth.client_id), valeoAuth.client_id },
                    { nameof(valeoAuth.client_secret), valeoAuth.client_secret },
                    { nameof(valeoAuth.grant_type), valeoAuth.grant_type },
                    { nameof(valeoAuth.scope), valeoAuth.scope }
                }
            )).Result;
            authData = responce.Content.ReadAsAsync<AuthorizationData>().Result;
            authData.expiration_date = DateTime.Now.AddSeconds(authData.expires_in - SafeExpirationGap);

        }
        private ConfigProvider _config;

        public ValeoAPIService(ConfigProvider config)
        {
            _config = config;
            HttpClientHandler hmh = new HttpClientHandler()
            {
                // Proxy = new WebProxy("http://proxy.isd.dp.ua:8080")
            };
            _client = new HttpClient(hmh);
            LoadAuth();
        }

        public async Task<List<Doctor>> GetDoctorsByCategory(string category)
        {

            string json = await Client.GetStringAsync(string.Format(_config.ValeoApi.Urls.Doctors, category))
                .ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<Doctor>>(json);
        }

        public async Task<List<Time>> GetFreeTimeByDoctor(string doctor)
        {
            string json = await Client.GetStringAsync(string.Format(_config.ValeoApi.Urls.Times, doctor))
                .ConfigureAwait(false);
            return JsonConvert.DeserializeObject<List<Time>>(json);
        }

        public async Task<bool> SaveOrder(Order order)
        {
            var response = await Client.PostAsJsonAsync(_config.ValeoApi.Urls.Save, order)
                .ConfigureAwait(false);
            return await response.Content.ReadAsAsync<bool>();
        }
    }
}