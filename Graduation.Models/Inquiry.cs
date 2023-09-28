using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Graduation.Models
{
    public class Inquiry
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
        public string Content { get; set; }
        [ValidateNever]
        public User User { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool? Reply { get; set; } = false;
        public DateTimeOffset? RepliedAt { get; set; }
    }
}
