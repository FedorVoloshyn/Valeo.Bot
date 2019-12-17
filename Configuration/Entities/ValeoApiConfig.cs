namespace Valeo.Bot.Configuration.Entities
{
    public class HelsiAPIConfig
    {
        public HelsiAPIAuth AuthData { get; set; }
        public ValeoUrls Urls { get; set; }
    }

    public class ValeoUrls
    {
        public string Token { get; set; }
        public string Auth { get; set; }
        public string Specialities { get; set; }
        public string Doctors { get; set; }
        public string DoctorInfo { get; set; }
        public string BlockedTimes { get; set; }
        public string Save { get; set; }
    }

    public class HelsiAPIAuth
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string grant_type { get; set; }
        public string scope { get; set; }
    }
}