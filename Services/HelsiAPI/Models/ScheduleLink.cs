using System;

namespace Valeo.Bot.Services.HelsiAPI.Models
{
    public class ScheduleLink
    {
        public string ScheduleId { get; set; }
        public string Cabinet { get; set; }
        public DateTime DateEnd { get; set; }
        public DateTime DateStart { get; set; }
    }
}