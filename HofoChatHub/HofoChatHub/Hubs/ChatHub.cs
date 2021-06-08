using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HofoChatHub.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string chatID, string userID, string message)
        {
            await Clients.Group(chatID).SendAsync("ReceiveMessage", userID, message);
        }
    }
}
