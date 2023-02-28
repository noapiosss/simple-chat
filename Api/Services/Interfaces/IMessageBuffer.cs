using Api.Models;

namespace Api.Services.Interfaces
{
    public interface IMessageBuffer
    {
        public void AddMessage(ChatMessage message);
        public List<ChatMessage> GetMessages();
    }
}