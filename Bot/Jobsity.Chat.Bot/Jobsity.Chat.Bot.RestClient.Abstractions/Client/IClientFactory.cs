namespace Jobsity.Chat.Bot.RestClient.Abstractions.Client
{
    public interface IClientFactory
    {
        Task<string> GetAsync(string stockCode);
    }
}
