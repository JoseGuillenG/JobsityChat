namespace Jobsity.Chat.Api.MessageBroker
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message);
    }
}