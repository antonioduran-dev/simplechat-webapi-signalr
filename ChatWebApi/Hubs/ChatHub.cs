using DB.Data;
using DB.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatWebApi.Hubs
{
    // inherit of Hub
    public class ChatHub : Hub
    {
        private readonly ChatContext _context;

        // inject the context in constructor
        public ChatHub(ChatContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string user, string message)
        {
            // create a new message
            var chatMessage = new Message
            {
                User = user,
                Text = message,
                Timestamp = DateTime.Now
            };

            // save the message in DB.
            _context.Messages.Add(chatMessage);
            await _context.SaveChangesAsync();

            // send the respons to all clients.
            await Clients.All.SendAsync("ReceiveMessage", user, message, chatMessage.Timestamp);
        }
    }
}
