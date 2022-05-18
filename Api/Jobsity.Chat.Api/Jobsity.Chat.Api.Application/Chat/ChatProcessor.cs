using Jobsity.Chat.Api.Models;
using Jobsity.Chat.Api.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace Jobsity.Chat.Api.Application.Chat
{
    public class ChatProcessor: IChatProcessor
    {
        private readonly IHubContext<ChatHub> _hub;

        public ChatProcessor(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }

        public async Task ProcessUserMessageAsync(ChatMessage newMessage)
        {
            try
            {
                await _hub.Clients.All.SendAsync("ReceiveMessage", newMessage.UserName, newMessage.Message);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task ProcessBotMessageAsync(ChatMessage message)
        {
            try
            {
                await _hub.Clients.All.SendAsync("ReceiveMessage", "Bot", message.Message);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
