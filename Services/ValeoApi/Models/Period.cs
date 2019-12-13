using System;

namespace Valeo.Bot.Services.ValeoApi.Models
{
    public class Period
    {
        public string SchedulePeriodId { get; set; }
        public string ScheduleId { get; set; }
        public int Day { get; set; }
        public string Parity { get; set; }
        public string Type { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public int Msg { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}