namespace Jobsity.Chat.Api.MessageBroker.Abstractions.Subscriber
{
    public interface IMessageSubscriber
    {
        Task RecieveMessageAsync();
    }
}