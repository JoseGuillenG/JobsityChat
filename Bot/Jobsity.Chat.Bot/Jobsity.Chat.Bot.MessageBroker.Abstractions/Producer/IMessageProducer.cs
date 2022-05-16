namespace Jobsity.Chat.Bot.MessageBroker.Producer.Abstractions
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message);
    }
}