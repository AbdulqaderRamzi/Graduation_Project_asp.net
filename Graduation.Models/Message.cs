using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Graduation.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? Image { get; set; }
        [DisplayName("Sent by")]
        public DateTimeOffset SendAt { get; set; }
        public User? User { get; set; }
        public Community? Community { get; set; }
        public int? ReplyToId { get; set; }
        [ForeignKey("ReplyToId")]
        public Message? ReplyTo { get; set; }
    }
}
