using Jobsity.Chat.Api.Application.Chat;
using Jobsity.Chat.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Jobsity.Chat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IChatProcessor _chatProcessor;

        internal const int _numberOfMessagesToReturn = 50;

        public MessageController(IChatProcessor chatProcessor)
        {
            _chatProcessor = chatProcessor;
        }

        [HttpPost(Name = "SendMessage")]
        public async Task<IActionResult> Post(ChatMessage newMessage)
        {
            newMessage.Id = Guid.NewGuid().ToString();
            newMessage.MessageDateTime = DateTime.Now;
            newMessage.Code = String.Empty;
            newMessage.ChatRoom = String.Empty;
            await _chatProcessor.ProcessMessageAsync(newMessage);

            return Ok();
        }

        [HttpGet(Name = "GetMessages")]
        public IActionResult Get()
        {
            var messages = _chatProcessor.GetMessagesAsText(_numberOfMessagesToReturn);
            return Ok(messages);
        }
    }
}
