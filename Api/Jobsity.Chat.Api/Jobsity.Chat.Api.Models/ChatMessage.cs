using System.ComponentModel.DataAnnotations;

namespace Jobsity.Chat.Api.Models
{
    public class ChatMessage
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Message { get; set; }
        public string Code { get; set; }
        [Required]
        public string ChatRoom { get; set; }
        public DateTime MessageDateTime { get; set; }
    }
}
