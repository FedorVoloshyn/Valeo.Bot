using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Valeo.Bot.Data.Entities
{        
    public class Registration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string AuthServiceToken { get; set; }
        public int? RegistrationMessageId { get; set; }
        public DateTime Time { get; set; }
    }
}