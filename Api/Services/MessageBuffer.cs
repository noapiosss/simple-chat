using Api.Models;
using Api.Services.Interfaces;

namespace Api.Services
{
    public class MessageBuffer : IMessageBuffer
    {
        private readonly Queue<ChatMessage> _messages = new(20);
        public void AddMessage(ChatMessage message)
        {
            if (_messages.Count == 20)
            {
                _ = _messages.Dequeue();
            }

            _messages.Enqueue(message);
        }

        public List<ChatMessage> GetMessages()
        {
            return _messages.ToList();
        }
    }
}