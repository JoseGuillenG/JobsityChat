using Jobsity.Chat.Api.Application.Chat;
using Jobsity.Chat.Api.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Jobsity.Chat.Api.MessageBroker.Producer
{
    public class RabbitMQSubscriber : IHostedService
    {
        private readonly IBotChatProcessor _chatProcessor;
        private IConnection _connection = null;
        private IModel _channel = null;

        public RabbitMQSubscriber(IBotChatProcessor chatProcessor)
        {
            _chatProcessor = chatProcessor;
        } 

        public Task StartAsync(CancellationToken cancelToken = default)
        {
            return Task.Run(async () => await Run(), cancelToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Dispose();
            _channel.Dispose();
            return Task.CompletedTask;
        }

        private Task Run()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare("messagesProccesed", exclusive: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += ProcessMessage;

            _channel.BasicConsume(queue: "messagesProccesed", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private async void ProcessMessage(object? model, BasicDeliverEventArgs args)
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var messageDeserialized = JsonConvert.DeserializeObject<ChatMessage>(message);

            Console.WriteLine($"Message received: {message}");

            await _chatProcessor.ProcessMessageAsync(messageDeserialized);
        }
    }
}
