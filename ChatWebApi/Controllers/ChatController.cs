using ChatWebApi.Hubs;
using ChatWebApi.Services;
using DB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        // create the chat service interface and Hub context interface.
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IChatService chatService, IHubContext<ChatHub> hubContext)
        {
            _chatService = chatService;
            _hubContext = hubContext;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Message>))]
        [HttpGet("messages")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            IEnumerable<Message> messages = await _chatService.GetAll();

            return Ok(messages);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("send")]
        public async Task<ActionResult> SendMessage([FromBody] Message message)
        {
            Message response = await _chatService.Post(message);

            await _hubContext.Clients.All.SendAsync("ReceiveMessage", response.User, response.Text, response.Timestamp);

            return Ok();
        }
    }
}
