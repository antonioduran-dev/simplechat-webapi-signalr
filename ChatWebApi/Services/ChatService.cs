using DB.Data;
using DB.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatWebApi.Services
{
    public class ChatService : IChatService
    {
        // create the context to inject in constructor.
        private readonly ChatContext _chatContext;

        public ChatService(ChatContext chatContext)
        {
            _chatContext = chatContext;
        }

        public async Task<IEnumerable<Message>> GetAll()
        {
            // return the messages in DB ordered by date.
            return await _chatContext.Messages.OrderBy(m => m.Timestamp).ToListAsync();
        }

        public async Task<Message> Post(Message model)
        {
            try
            {
                // set the datetime in the message TimeStamp.
                model.Timestamp = DateTime.Now;
                // save the message in DB.
                _chatContext.Messages.Add(model);
                await _chatContext.SaveChangesAsync();

                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
