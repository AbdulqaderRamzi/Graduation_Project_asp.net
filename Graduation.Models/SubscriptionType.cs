using System.ComponentModel.DataAnnotations;

namespace Graduation.Models
{
    public class SubscriptionType
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public List<Subscription>? Subscriptions { get; set; } = new List<Subscription>();
        public int Solution { get; set; } = 0;
        public int Explanation { get; set; } = 0;
    }
}
