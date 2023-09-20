using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Role { get; set; }
        public ChatRoom ChatRoom { get; set; }
        public List<Message>? Messages { get; set; } = new List<Message>();
        public Community? Community { get; set; }
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public List<Applicant> Applicants { get; set; } = new List<Applicant>();
        public int Solution { get; set; } = 0;
        public int Explanation { get; set; } = 0;
    }
}
