using System;
using System.Collections.Generic;

namespace Valeo.Bot.Services.HelsiAPI.Models
{
    public struct Doctor
    {
        public string ResourceId { get; set; }
        public string ResourceType { get; set; }  
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Available { get; set; }
        public bool Sex { get; set; }
        public int Time_slot { get; set; }
        public string PhotoId { get; set; }
        public List<Period> Period { get; set; }
        public Position Position { get; set; }
        public List<ScheduleLink> ScheduleLink { get; set; }
        public List<Speciality> Speciality { get; set; }
        public List<string> Types { get; set; }
        public Organization Organization { get; set; }
        public Division Division { get; set; }
        public Subdivision Subdivision { get; set; }
        public DateTime WorkStartDate { get; set; }
        public List<string> Event { get; set; }
        public List<string> Rules { get; set; }
        public List<string> ContactPhones { get; set; }
        public List<ResourceNA> ResourceNA { get; set; }
    }

    public class Position
    {
        public string PositionId { get; set; }
        public string Name { get; set; }
    }
}