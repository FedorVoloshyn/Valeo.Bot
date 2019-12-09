using System;

namespace Valeo.Bot.Services.ValeoApi.Models
{
    public class AuthorizationData
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public DateTime expiration_date { get; set; }
        public string token_type { get; set; }
    }
}