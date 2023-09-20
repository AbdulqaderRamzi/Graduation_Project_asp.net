using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; }
        public int SubscriptionTypeId { get; set; }
        [ForeignKey("SubscriptionTypeId")]
        [ValidateNever]
        public SubscriptionType SubscriptionType { get; set; }
    }
}
