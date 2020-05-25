using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;

namespace SignalR
{
   public class User
    {
       
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public ICollection<Connection> Connections { get; set; }
          
         public virtual ICollection<UserRooms> Rooms { get; set; }

    }
}
