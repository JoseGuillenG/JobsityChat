namespace Jobsity.Chat.Bot.Models.Messages
{
    public class ChatMessage
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public string ChatRoom { get; set; }
        public DateTime MessageDateTime { get; set; }
    }
}
