namespace Jobsity.Chat.Bot.Application.Stock
{
    public interface IStockProcessor
    {
        Task ProcessStockMessageAsync(string stockCode);
    }
}
