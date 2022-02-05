using Queue.Data;

namespace Queue.Service;

public class QueueService : BackgroundService
{
    private readonly ILogger<QueueService> _logger;
    private readonly AppDbContext _dbcontext;

    public QueueService(ILogger<QueueService> logger, AppDbContext dbContext)
    {
        _logger = logger;
        _dbcontext = dbContext;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            var queues = _dbcontext.Queues.Where(q => q.ExpirationTime < DateTimeOffset.UtcNow).ToList();
            foreach(var obj in queues)
            {
               obj.IsActive = false;
            }
            _dbcontext.UpdateRange(queues);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
