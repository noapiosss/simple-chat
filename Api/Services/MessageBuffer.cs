using Api.Services.Interfaces;

namespace Api.Services
{
    public class MessageBuffer : IMessageBuffer
    {
        private readonly Queue<string> _messages = new(20);
        public void AddMessage(string message)
        {
            if (_messages.Count == 20)
            {
                _ = _messages.Dequeue();
            }

            _messages.Enqueue(message);
        }

        public List<string> GetMessages()
        {
            return _messages.ToList();
        }
    }
}