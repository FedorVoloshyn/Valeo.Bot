namespace ValeoBot.Configuration.Entities
{
    public class ValeoApiConfig
    {
        public ValeoApiAuth AuthData { get; set; }
        public ValeoUrls Urls { get; set; }
    }

    public class ValeoUrls
    {
        public string Token { get; set; }
        public string Auth { get; set; }
        public string Specialities { get; set; }
        public string Doctors { get; set; }
        public string DoctorInfo { get; set; }
        public string Times { get; set; }
        public string Save { get; set; }
    }

    public class ValeoApiAuth
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string grant_type { get; set; }
        public string scope { get; set; }
    }
}