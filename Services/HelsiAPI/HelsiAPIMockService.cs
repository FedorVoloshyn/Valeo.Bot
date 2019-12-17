using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Valeo.Bot.Configuration;
using Valeo.Bot.Data.Entities;
using Valeo.Bot.Services.HelsiAPI.Models;

namespace Valeo.Bot.Services.HelsiAPI
{
    public class HelsiAPIMockService : IHelsiAPIService
    {
        public async Task<Doctor> GetDoctor(string doctorId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Doctor>> GetDoctors(int limit = 10, string specialityId = "")
        {
            throw new NotImplementedException();
        }

        public async Task<List<TimeSlot>> GetFreeTimeByDoctor(string doctorId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Speciality>> GetOrganizationSpecialities()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveTime(Order order)
        {
            throw new NotImplementedException();
        }
    }
}