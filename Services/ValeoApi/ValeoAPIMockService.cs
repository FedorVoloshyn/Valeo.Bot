using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Valeo.Bot.Configuration;
using Valeo.Bot.Data.Entities;
using Valeo.Bot.Services.ValeoApi.Models;

namespace Valeo.Bot.Services.ValeoApi
{
    public class ValeoAPIMockService : IValeoAPIService
    {
        private readonly HttpClient _client;

        public async Task<List<Doctor>> GetDoctorsByCategory(string category)
        {
            var doctors = new List<Doctor>();
            for (int i = 0; i < 20; i++)
            {
                doctors.Add(new Doctor() { FirstName = "Игорь", LastName = "Романов" });
            }
            return doctors;
        }

        public async Task<List<TimeSlot>> GetFreeTimeByDoctor(string doctor)
        {
            var times = new List<TimeSlot>();
            for (int i = 0; i < 20; i++)
            {
                times.Add(new TimeSlot() { Start = DateTime.Now });
            }
            return times;
        }

        public async Task<bool> SaveOrder(Order order)
        {
            return true;
        }
    }
}