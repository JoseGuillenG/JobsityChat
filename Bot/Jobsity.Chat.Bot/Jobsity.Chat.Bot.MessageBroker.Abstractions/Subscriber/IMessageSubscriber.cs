namespace Jobsity.Chat.Bot.MessageBroker.Subscriber.Abstractions
{
    public interface IMessageSubscriber
    {
        T RecieveMessage<T>();
    }
}