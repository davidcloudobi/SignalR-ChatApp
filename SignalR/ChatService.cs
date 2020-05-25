using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SignalR
{
   public class ChatService:IChatService
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly ChatDbContent _chatDbContent;

        public ChatService(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, ChatDbContent chatDbContent)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _chatDbContent = chatDbContent;
        }


        public async Task  SaveChat(ChatMessage message)
        {
          await  _chatDbContent.Message.AddAsync(message);
          await  _chatDbContent.SaveChangesAsync();

        }

        public async Task<List<ChatMessage>> OutChat()
        {
            var res = await _chatDbContent.Message.ToListAsync();
            return res;
        }

    }
}
