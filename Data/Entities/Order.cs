using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Valeo.Bot.Data.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ChatId { get; set; }
        public string Category { get; set; }
        public string DoctorId { get; set; }
        public string Time { get; set; }

        public override string ToString()
        {
            return $"Order: Id = {Id}, ChatId = {ChatId}, Category = {Category}, DoctorID = {DoctorId}, Time = {Time}";
        }
    }
}