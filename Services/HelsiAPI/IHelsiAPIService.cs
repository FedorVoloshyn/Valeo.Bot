using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Valeo.Bot.Data.Entities;
using Valeo.Bot.Services.HelsiAPI.Models;

namespace Valeo.Bot.Services.HelsiAPI
{
    public interface IHelsiAPIService
    {
         Task<List<Speciality>> GetOrganizationSpecialities();
         Task<List<Doctor>> GetDoctors(int limit = 10, string specialityId = "");
         Task<Doctor> GetDoctor(string doctorId);
         Task<List<TimeSlot>> GetFreeTimeByDoctor(string doctorId, DateTime date);
         Task<bool> SaveTime(Order order);



    }
}