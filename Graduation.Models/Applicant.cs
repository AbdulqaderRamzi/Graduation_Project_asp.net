using System.ComponentModel.DataAnnotations;

namespace Graduation.Models
{
    public class Applicant
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? CV { get; set; }
        public User User { get; set; }
        public DateTimeOffset AppliedAt { get; set; }
    }
}
