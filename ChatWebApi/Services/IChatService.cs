using DB.Models;

namespace ChatWebApi.Services
{
    public interface IChatService
    {
        Task<IEnumerable<Message>> GetAll();
        Task<Message> Post(Message model);
    }
}
