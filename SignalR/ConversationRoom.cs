using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SignalR
{
    public class ConversationRoom
    {
  [Key]
        public int ConversationRoomId { get; set; }
        public string RoomName { get; set; }
        public virtual ICollection<UserRooms> Users { get; set; }
    }
}

