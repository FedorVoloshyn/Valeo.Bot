using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ValeoBot.Data.Entities
{        
    public class Registration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public string AuthServiceToken { get; set; }
        public int? RegistrationMessageId { get; set; }
        public DateTime Time { get; set; }
    }
}