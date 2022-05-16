using Jobsity.Chat.Bot.Application.Stock;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Jobsity.Chat.Bot.MessageBroker.Producer
{
    public class RabbitMQSubscriber: IHostedService
    {
        private readonly IStockProcessor _stockProcessor;
        private IConnection _connection = null;
        private IModel _channel = null;

        public RabbitMQSubscriber(IStockProcessor stockProcessor)
        {
            _stockProcessor = stockProcessor;
        }

        private void Run()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare("messages", exclusive: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += ProcessMessage;

            _channel.BasicConsume(queue: "messages", autoAck: true, consumer: consumer);

            Console.ReadKey();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.Run();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _connection.Dispose();
            _channel.Dispose();
            return Task.CompletedTask;
        }

        private async void ProcessMessage(object? model, BasicDeliverEventArgs args)
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"Message received: {message}");

            await _stockProcessor.ProcessStockMessageAsync(message);
        }
    }
}
