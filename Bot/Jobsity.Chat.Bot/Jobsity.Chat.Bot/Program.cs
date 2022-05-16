using Jobsity.Chat.Bot.MessageBroker.Producer;
using Jobsity.Chat.Bot.MessageBroker.Producer.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder();
host.ConfigureServices((hostContext, services) =>
{
    services.AddScoped<IMessageProducer, RabbitMQProducer>();
    services.AddHostedService<RabbitMQSubscriber>();
});
host.RunConsoleAsync();