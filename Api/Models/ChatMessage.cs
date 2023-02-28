namespace Api.Models
{
    public struct ChatMessage
    {
        public string Username { get; set; }
        public string Time { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
    }
}