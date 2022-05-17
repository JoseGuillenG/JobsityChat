namespace Jobsity.Chat.Api.MessageBroker.Abstractions.Producer
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message);
    }
}