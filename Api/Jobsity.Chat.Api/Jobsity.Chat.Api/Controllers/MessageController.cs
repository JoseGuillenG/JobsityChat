using Jobsity.Chat.Api.MessageBroker.Abstractions.Producer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jobsity.Chat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageProducer _messageProducer;

        public MessageController(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
        }

        [HttpPost(Name = "SendMessage")]
        public async Task<IActionResult> Post(string newMessage)
        {
            _messageProducer.SendMessage(newMessage);
            return Ok();
        }
    }
}
