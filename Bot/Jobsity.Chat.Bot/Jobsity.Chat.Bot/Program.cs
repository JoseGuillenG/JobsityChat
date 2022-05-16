using Jobsity.Chat.Bot.Application.Stock;
using Jobsity.Chat.Bot.MessageBroker.Producer;
using Jobsity.Chat.Bot.MessageBroker.Producer.Abstractions;
using Jobsity.Chat.Bot.RestClient.Abstractions.Client;
using Jobsity.Chat.Bot.RestClient.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder();
host.ConfigureServices((hostContext, services) =>
{
    services.AddScoped<IMessageProducer, RabbitMQProducer>();
    services.AddSingleton<IClientFactory, ClientFactory>();
    services.AddSingleton<IStockProcessor, StockProcessor>();
    services.AddHostedService<RabbitMQSubscriber>();
    services.AddHttpClient();
});

host.RunConsoleAsync();