
using Queue.Models;

namespace Queue.Data;
public class AppDbContext : IdentityDbContext<UserModel> 
{
    public AppDbContext (DbContextOptions<AppDbContext> options) : base (options) { }
    public DbSet<QueueModel> Queues { get; set; }

}
