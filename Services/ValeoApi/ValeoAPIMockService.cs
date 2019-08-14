using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ValeoBot.Configuration;
using ValeoBot.Services.ValeoApi.Models;

namespace ValeoBot.Services.ValeoApi
{
    public class ValeoAPIMockService : IValeoAPIService
    {
        private readonly HttpClient _client;
        private ConfigProvider _config;

        public ValeoAPIMockService(ConfigProvider config)
        {
        }

        public async Task<List<Doctor>> GetDoctorsByCategory(string category)
        {
            var doctors = new List<Doctor>();
            for(int i = 0; i < 20; i++)
            {
                doctors.Add(new Doctor() { FirstName = "Игорь", LastName = "Романов"});
            }
            return doctors;
        }

        public async Task<List<Time>> GetFreeTimeByDoctor(string doctor)
        {
            var times = new List<Time>();
            for(int i = 0; i < 20; i++)
            {
                times.Add(new Time() { Value = DateTime.Now });
            }
            return times;
        }
    }
}