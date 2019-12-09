using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Valeo.Bot.Data.Entities
{

    public class ValeoUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [ForeignKey("Order")]
        public long? LastOrderId { get; set; }
        public Order Order { get; set; }

        public bool IsAdmin { get; set; }
    }
}