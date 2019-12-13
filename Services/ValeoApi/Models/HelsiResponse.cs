using System.Collections.Generic;

namespace Valeo.Bot.Services.ValeoApi.Models
{
    public class HelsiResponse<TData>
    {
        public TData Data { get; set; }
        public Paging Paging { get; set; }
    }

    public class Paging { 
        public string Length { get; set; }
        public string Page { get; set; }
        public string Limit { get; set; }
    }
}