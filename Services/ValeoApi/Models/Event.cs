using System;

namespace Valeo.Bot.Services.ValeoApi.Models
{
    public struct Event
    {
        public DateTime BeginningAt { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
    }
}