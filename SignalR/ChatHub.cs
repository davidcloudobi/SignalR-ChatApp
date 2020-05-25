using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace SignalR
{
  //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
 // [Authorize]
    public  class ChatHub:Hub
    {
        private readonly IChatService _chatService;
        private readonly ChatDbContent db;
      //  private readonly HttpContextAccessor _httpContextAccessor;

        public ChatHub(IChatService chatService, ChatDbContent db)
        {
            _chatService = chatService;
            this.db = db;
          //  _httpContextAccessor = httpContextAccessor;
        }

        public async Task JoinRoom(string roomName)
        {
             await  Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            var ListMsg =  new List<ChatMessage>();
             var newMsg = new ChatMessage {Message = Context.User.Identity.Name + " joined"};
             ListMsg.Add(newMsg);
             await  Clients.Group(roomName).SendAsync("ReceiveMessage", ListMsg);
             IPrincipal hh = new ClaimsPrincipal();
           
             //  IPrincipal.Identity.Name = roomName;
        }

        public async Task LeaveRoom(string roomName)
        {
             await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task SendMessage(ChatMessage msg)
        {
            //msg.Id = Guid.NewGuid();
           await _chatService.SaveChat(msg);
           var message = await _chatService.OutChat();
             await Clients.All.SendAsync("ReceiveMessage", message);

            // await  Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", msg);
           
            //await Clients.All.SendAsync( message);
            await  Clients.Group("user").SendAsync("ReceiveMessage", message);
        }



        //####################################################################################




        
//using Microsoft.AspNet.SignalR;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Threading.Tasks;

//namespace GroupsExample
   // {
     //   [Authorize]
       // public class ChatHub : Hub
       // {
           // public async override  Task OnConnected()
           // public virtual Task OnConnected()
            public async Task OnConnected()
        {
            // var username = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            //using (var db = new UserContext())
            //{
            // Retrieve user.


            var use = await db.User.FirstOrDefaultAsync(x => x.UserName == Context.User.Identity.Name);

            var user = await db.User.Include(u => u.Rooms)
                        .FirstOrDefaultAsync(x => x.UserName == Context.User.Identity.Name);
            //var user = await db.User.Include(u => u.Rooms)
            //    .FirstOrDefaultAsync(x => x.UserName ==username);
            //.Include(u => u.Rooms)
            //.SingleOrDefault(u => u.UserName == Context.User.Identity.Name);

            // If user does not exist in database, must add.
            if (user == null)
                    {


                   var   newUser = new User()
                  {
                       UserName = Context.User.Identity.Name
                      // UserName = username
                   };
                db.User.Add(newUser);
                db.SaveChanges();
                }
                    else
                    {
                        // Add to each assigned group.
                        foreach (var item in user.Rooms)
                        {
                            await Groups.AddToGroupAsync(Context.ConnectionId, item.Room.RoomName);
                    //await  Groups.Add(Context.ConnectionId, item.RoomName);
                        }
                    }
            // }

            var ListMsg = new List<ChatMessage>();
            var newMsg = new ChatMessage { Message = Context.User.Identity.Name + " is connected" };
            ListMsg.Add(newMsg);
           // await Clients.Groups(item.Room.RoomName).SendAsync("ReceiveMessage", ListMsg);
            await Clients.All.SendAsync("ReceiveMessage", ListMsg);
           // return base.OnConnected();
            }

            public async Task AddToRoom(string roomName)
            {
          //  var username = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;


            // Retrieve room.
            var room = await db.Rooms.FirstOrDefaultAsync(x => x.RoomName == roomName);

                    if (room != null) 
                    {
                       // var user = new User() { UserName =username };
                        var user = new User() { UserName = Context.User.Identity.Name };
                var newUser = new UserRooms()
                        {
                            User = user
                        };
                        db.User.Attach(user);
                room.Users.Add(newUser);
                       // room.Users.Add(user);
                        db.SaveChanges();
                     await   Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                    }
                
            }

            public async Task RemoveFromRoom(string roomName)
            {
               
                
                    // Retrieve room.
                    var room = await db.Rooms.FindAsync(roomName);
                   // var test = await  db.Rooms.FirstOrDefaultAsync(x=> x.)

                    if (room != null)
                    {
                        var user = new User() { UserName = Context.User.Identity.Name };
                        var newUser = new UserRooms()
                        {
                            User = user
                        };
                db.User.Attach(user);

                        room.Users.Remove(newUser);
                        db.SaveChanges();

                      //  Groups.Remove(Context.ConnectionId, roomName);
                    await    Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
            }
                
            }
       // }
   // }
}
}
