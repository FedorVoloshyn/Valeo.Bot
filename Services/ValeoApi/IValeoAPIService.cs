using System.Threading.Tasks;

namespace ValeoBot.Services.ValeoApi
{
    interface IValeoAPIService
    {
        Task<Doctor> GetDoctorsByCategory(string category);
        Task<Time[]> GetFreeTimeByDoctor(Doctor doctor);
    }
}