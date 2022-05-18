using Jobsity.Chat.Api.Application.Chat;
using Jobsity.Chat.Api.MessageBroker.Abstractions.Producer;
using Jobsity.Chat.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Jobsity.Chat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageProducer _messageProducer;
        private readonly IChatProcessor _chatProcessor;

        public MessageController(IChatProcessor chatProcessor, IMessageProducer messageProducer)
        {
            _chatProcessor = chatProcessor;
            _messageProducer = messageProducer;
        }

        [HttpPost(Name = "SendMessage")]
        public async Task<IActionResult> Post(ChatMessage newMessage)
        {
            newMessage.MessageDateTime = DateTime.Now;

            await _chatProcessor.ProcessUserMessageAsync(newMessage);

            string pattern = @"^(\/stock=).*";
            Regex regex = new Regex(pattern);

            if (regex.IsMatch(newMessage.Message))
            {
                var messageWithoutSpaces = newMessage.Message.Split(' ')[0];
                var code = messageWithoutSpaces.Remove(0, 7);
                newMessage.Code = code;
                _messageProducer.SendMessage(newMessage);
            }

            return Ok();
        }
    }
}
