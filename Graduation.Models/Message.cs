using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Graduation.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        public DateTimeOffset SendAt { get; set; }
        [DisplayName("Sent by")]
        public User User { get; set; }
        public Community? Community { get; set; }
        public Message? ReplyTo { get; set; }
    }
}
