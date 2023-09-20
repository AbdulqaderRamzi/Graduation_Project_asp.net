using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Models
{
    public class Donation
    {
        [Key]
        public int Id { get; set; }
        public int Amount { get; set; }
        public DateTimeOffset DonatedAt { get; set; }
    }
}
