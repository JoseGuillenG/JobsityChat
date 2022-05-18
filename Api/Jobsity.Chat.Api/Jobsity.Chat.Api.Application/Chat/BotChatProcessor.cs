using Jobsity.Chat.Api.Models;
using Jobsity.Chat.Api.Persistance;
using Jobsity.Chat.Api.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Jobsity.Chat.Api.Application.Chat
{
    public class BotChatProcessor : IBotChatProcessor
    {
        private readonly IHubContext<ChatHub> _hub;

        public BotChatProcessor(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }

        public async Task ProcessMessageAsync(ChatMessage message)
        {
            try
            {
                message.MessageDateTime = DateTime.Now;
                message.UserName = "Bot";
                message.Id = Guid.NewGuid().ToString();
                var messageToSend = message.MessageDateTime.ToString() + " " + message.UserName;
                await _hub.Clients.All.SendAsync("ReceiveMessage", messageToSend, message.Message);
                SaveMessage(message);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        // ToDo: Sorry for this implementation, I would find a different way to do it with more time
        // Reason: I used an independent IHostedService to subscribe the API to the RabbitMQ queue 
        // The IHostedServices are singletons, so I wasn't able to inject the context here
        // since contexts are scoped, but in order to save the bot messages as well, I went on this approach
        private void SaveMessage(ChatMessage message)
        {
            var options = new DbContextOptionsBuilder<ChatContext>()
                .UseInMemoryDatabase(databaseName: "ChatMessagesDB")
                .Options;

            using (var context = new ChatContext(options))
            {
                context.Messages.Add(message);
                context.SaveChanges();
            }
        }
    }
}
