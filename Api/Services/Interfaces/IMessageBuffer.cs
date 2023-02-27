namespace Api.Services.Interfaces
{
    public interface IMessageBuffer
    {
        public void AddMessage(string message);
        public List<string> GetMessages();
    }
}