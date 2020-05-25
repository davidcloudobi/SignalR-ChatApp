using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SignalR
{
 public   class UserRooms
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int ConversationRoomId { get; set; }
        [ForeignKey("ConversationRoomId")]
        public ConversationRoom Room { get; set; }
    }
}
