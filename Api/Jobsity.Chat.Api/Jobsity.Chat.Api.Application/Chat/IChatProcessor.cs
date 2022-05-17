namespace Jobsity.Chat.Api.Application.Chat
{
    public interface IChatProcessor
    {
        Task ProcessMessageAsync(string user, string messageToSend);
    }
}
