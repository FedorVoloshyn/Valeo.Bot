using System.Collections.Generic;
using System.Threading.Tasks;
using Valeo.Bot.Data.Entities;
using Valeo.Bot.Services.ValeoApi.Models;

namespace Valeo.Bot.Services.ValeoApi
{
    public interface IValeoAPIService
    {
        Task<List<Doctor>> GetDoctorsByCategory(string category);
        Task<List<Time>> GetFreeTimeByDoctor(string doctor);
        Task<bool> SaveOrder(Order order);
    }
}