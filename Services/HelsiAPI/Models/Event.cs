﻿using System;

namespace Valeo.Bot.Services.HelsiAPI.Models
{
    public struct Event
    {
        public DateTime BeginningAt { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
    }
}