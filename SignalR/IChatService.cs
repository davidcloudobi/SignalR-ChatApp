using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR
{
    public interface IChatService
    {
        Task SaveChat(ChatMessage message);
        Task<List<ChatMessage>> OutChat();


    }
}