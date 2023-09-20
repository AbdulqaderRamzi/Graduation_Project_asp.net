using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graduation.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public int ChatRoomMessageId { get; set; }
        [ForeignKey(nameof(ChatRoomMessageId))]
        public ChatRoomMessage ChatRoomMessage { get; set; }
        public string ImageUrl { get; set; }
    }
}
