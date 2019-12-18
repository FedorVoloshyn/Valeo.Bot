using System.Collections.Generic;

namespace Valeo.Bot.Services.HelsiAPI.Models
{
    public class Subdivision
    {
        public List<string> Integrations { get; set; }
        public Rating Rating { get; set; }
        public string Name { get; set; }
        public Addresses Addresses { get; set; }
        public string PropertyType { get; set; }
    }

    public class Rating
    {
        public float Professionalism { get; set; }
        public float Service { get; set; }
        public float Average { get; set; }
        public int ReviewCount { get; set; }
        public int CommentsCount { get; set; }
        public bool IsPublic { get; set; }

    }
}