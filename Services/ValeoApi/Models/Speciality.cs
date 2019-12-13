using System.Collections.Generic;

namespace Valeo.Bot.Services.ValeoApi.Models
{
    public struct Speciality
    {
        public string Name { get; set; }
        public string DoctorSpeciality { get; set; }
        public string ShortName { get; set; }
        public string SpecialityId { get; set; }
        public List<SpecialityTag> SearchTagsSpeciality { get; set; }
    }

    public class SpecialityTag
    {
        public string Name { get; set; }
        public string SearchTagsSpecialityId { get; set; }
    }
}