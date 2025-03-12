using Microsoft.AspNetCore.SignalR;

namespace ASH.Services
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.Others.SendAsync("ReceiveMessage", user, message, false);
        }
    }
}