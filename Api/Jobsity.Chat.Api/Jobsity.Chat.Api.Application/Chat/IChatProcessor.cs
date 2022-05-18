using Jobsity.Chat.Api.Models;

namespace Jobsity.Chat.Api.Application.Chat
{
    public interface IChatProcessor
    {
        Task ProcessUserMessageAsync(ChatMessage newMessage);
        Task ProcessBotMessageAsync(ChatMessage message);
    }
}
