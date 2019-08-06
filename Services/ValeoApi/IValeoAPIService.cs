using System.Threading.Tasks;
using ValeoBot.Services.ValeoApi.Models;

namespace ValeoBot.Services.ValeoApi
{
    interface IValeoAPIService
    {
        Task<Doctor> GetDoctorsByCategory(string category);
        Task<Time[]> GetFreeTimeByDoctor(Doctor doctor);
    }
}