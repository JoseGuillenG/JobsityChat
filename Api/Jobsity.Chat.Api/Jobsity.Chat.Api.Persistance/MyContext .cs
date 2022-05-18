using Jobsity.Chat.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Jobsity.Chat.Api.Persistance
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        { }
        public DbSet<ChatMessage> Customers { get; set; }
    }
}