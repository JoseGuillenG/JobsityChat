using Jobsity.Chat.Bot.MessageBroker.Producer.Abstractions;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Jobsity.Chat.Bot.MessageBroker.Producer
{
    public class RabbitMQProducer: IMessageProducer
    {
        public void SendMessage<T>(T message)
        {
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost"
                };

                var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare("messagesProccesed", exclusive: false);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: "messagesProccesed", body: body);
            }
        }
    }
}
