using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ValeoBot.Data.Entities
{        
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public long ChatId { get; set; }
        public string DoctorID { get; set; }
        public string Time { get; set; }
    }
}