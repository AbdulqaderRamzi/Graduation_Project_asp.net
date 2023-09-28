using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation.Models
{
    public class ChatRoom
    {
        [Key]
        public int Id { get; set; }
        public User User { get; set; }
        public List<ChatRoomMessage>? ChatRoomMessages { get; set; } = new List<ChatRoomMessage>();
    }
}
