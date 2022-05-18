using Jobsity.Chat.Api.Models;

namespace Jobsity.Chat.Api.Application.Chat
{
    public interface IChatProcessor
    {
        Task ProcessMessageAsync(ChatMessage newMessage);
        List<string> GetMessagesAsText(int numberOfMessages);
    }
}
