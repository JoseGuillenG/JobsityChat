using Jobsity.Chat.Bot.Models.Messages;

namespace Jobsity.Chat.Bot.Application.Stock
{
    public interface IStockProcessor
    {
        Task ProcessStockMessageAsync(ChatMessage message);
    }
}
