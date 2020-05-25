using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SignalR
{
   public class ChatMessage
   {
      
     //  [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
        public Guid Id { get; set; }
        public string ClientUniqueId { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string FromUserName { get; set; }
        public DateTime Date { get; set; }
    }
}
