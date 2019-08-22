using System.Collections.Generic;
using System.Threading.Tasks;
using ValeoBot.Data.Entities;
using ValeoBot.Services.ValeoApi.Models;

namespace ValeoBot.Services.ValeoApi
{
    public interface IValeoAPIService
    {
        Task<List<Doctor>> GetDoctorsByCategory(string category);
        Task<List<Time>> GetFreeTimeByDoctor(string doctor);
        Task<bool> SaveOrder(Order order);
    }
}