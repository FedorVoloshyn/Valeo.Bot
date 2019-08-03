using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ValeoBot.Data.Entities
{
    
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Nickname { get; set; }
        
        [ForeignKey("Order")]
        public long? LastOrderId {get; set;}
        public Order Order { get; set; }

        public bool IsAdmin { get; set; }

        public User()
        {

        }

        public User(User user)
        {
            this.Nickname = user.Nickname;
            this.LastOrderId = user.LastOrderId;
        }
    }
}