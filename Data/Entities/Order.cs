using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ValeoBot.Data.Entities
{        
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long? ChatId { get; set; }
        public string DoctorID { get; set; }
        public string Time { get; set; }
        public string PatientName { get; set; }
        public string PhoneNumber {get; set;}
        public ICollection<User> Users { get; set; }
    }
}