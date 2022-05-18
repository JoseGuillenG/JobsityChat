using Jobsity.Chat.Api.MessageBroker.Abstractions.Producer;
using Jobsity.Chat.Api.Models;
using Jobsity.Chat.Api.Persistance;
using Jobsity.Chat.Api.SignalR;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace Jobsity.Chat.Api.Application.Chat
{
    public class ChatProcessor: IChatProcessor
    {
        private readonly IMessageProducer _messageProducer;
        private readonly IHubContext<ChatHub> _hub;
        private readonly ChatContext _context;

        public ChatProcessor(IHubContext<ChatHub> hub, ChatContext context, IMessageProducer messageProducer)
        {
            _hub = hub;
            _context = context;
            _messageProducer = messageProducer;
        }

        public List<string> GetMessagesAsText(int numberOfMessages)
        {
            try
            {
                var messages = _context.Messages.OrderByDescending(x => x.MessageDateTime).Take(numberOfMessages);
                return messages.Select(x => x.MessageDateTime.ToString() + " " + x.UserName + " says " + x.Message).ToList();
            }
            catch(Exception ex)
            {
                return new List<string>();
            }
        }

        public async Task ProcessMessageAsync(ChatMessage newMessage)
        {
            try
            {
                await SendMessageToChatAsync(newMessage);
                SendMessageToBotIfApplies(newMessage);
                SaveMessage(newMessage);

            }
            catch (Exception ex)
            {

            }
        }

        private async Task SendMessageToChatAsync(ChatMessage newMessage)
        {
            var messageToSend = newMessage.MessageDateTime.ToString() + " " + newMessage.UserName;
            await _hub.Clients.All.SendAsync("ReceiveMessage", messageToSend, newMessage.Message);
        }

        private void SendMessageToBotIfApplies(ChatMessage newMessage)
        {
            string pattern = @"^(\/stock=).*";
            Regex regex = new Regex(pattern);

            if (regex.IsMatch(newMessage.Message))
            {
                var messageWithoutSpaces = newMessage.Message.Split(' ')[0];
                var code = messageWithoutSpaces.Remove(0, 7);
                newMessage.Code = code;
                _messageProducer.SendMessage(newMessage);
            }
        }

        private void SaveMessage(ChatMessage newMessage)
        {
            _context.Add(newMessage);
            _context.SaveChanges();
        }
    }
}
