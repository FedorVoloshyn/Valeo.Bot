using System;
using System.Collections.Generic;

namespace Valeo.Bot.Services.HelsiAPI.Models
{
    public class ResourceNA
    {
        public string naId { get; set; }
        public string typeNAId { get; set; }
        public List<string> replacementDoctors { get; set; }
        public DateTime DateStartNA { get; set; }
        public DateTime DateStopNA { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}