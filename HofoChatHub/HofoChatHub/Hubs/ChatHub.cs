using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HofoChatHub.Hubs
{
    public class ChatHub : Hub
    {
        public Task JoinGroup(string chatId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task SendMessage(string chatId, string senderId, string message)
        {
            await Clients.Group(chatId).SendAsync("ReceiveMessage", senderId, message);
        }  
    }
}
