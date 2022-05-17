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

        public async Task ProcessMessageAsync(string user, string messageToSend)
        {
            try
            {
                await _hub.Clients.All.SendAsync("ReceiveMessage", user, messageToSend);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
