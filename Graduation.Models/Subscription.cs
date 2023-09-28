using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graduation.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }
        [ValidateNever]
        public User User { get; set; }
        [ValidateNever]
        public SubscriptionType SubscriptionType { get; set; }
    }
}
