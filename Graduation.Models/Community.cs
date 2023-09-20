using System.ComponentModel.DataAnnotations;

namespace Graduation.Models
{
    public class Community
    {
        [Key]
        public int Id { get; set; }
        public List<User>? Users { get; set; } = new List<User>();
        public List<Message>? Messages { get; set; } = new List<Message>();
    }
}
