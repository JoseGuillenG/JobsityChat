using Jobsity.Chat.Api.MessageBroker.Abstractions.Producer;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Jobsity.Chat.Api.MessageBroker.Producer
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

                channel.QueueDeclare("messages", exclusive: false);

                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "", routingKey: "messages", body: body);
            }
        }
    }
}
