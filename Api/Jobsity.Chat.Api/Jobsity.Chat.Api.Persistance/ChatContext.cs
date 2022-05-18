using Jobsity.Chat.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Jobsity.Chat.Api.Persistance
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options)
            : base(options)
        { }
        public DbSet<ChatMessage> Messages { get; set; }
    }
}