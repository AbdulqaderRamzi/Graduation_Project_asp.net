using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation.Models
{
    public class ChatRoomMessage
    {
        [Key]
        public int Id { get; set; }
        public string? Content { get; set; }
        public List<Image>? Images { get; set; } = new List<Image>();
        public string? Video { get; set; }
        public DateTimeOffset SendAt { get; set; }
        [DisplayName("Sent by")]
        public User User { get; set; }
        public ChatRoom? ChatRoom { get; set; }
        public bool Explanation { get; set; } = false;
        public bool Solution { get; set; } = false;
        public bool Closed { get; set; } = false;
    }
}
