using System;

namespace Valeo.Bot.Services.HelsiAPI.Models
{
    public struct TimeSlot
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public override string ToString()
        {
            return Start.ToShortTimeString();
        }
    }
}