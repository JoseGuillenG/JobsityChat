
using Jobsity.Chat.Api.Models;

namespace Jobsity.Chat.Api.Application.Chat
{
    public  interface IBotChatProcessor
    {
        Task ProcessMessageAsync(ChatMessage message);
    }
}
