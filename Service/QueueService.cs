using Queue.Data;

namespace Queue.Service;

public class QueueService : BackgroundService
{
    private readonly ILogger _logger;
    private readonly AppDbContext _dbcontext;

    public QueueService(ILogger logger, AppDbContext dbContext)
    {
        _logger=logger;
        _dbcontext=dbContext;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(true)
        {
            var queues = await _dbcontext.Queues.Where(q => q.ExpirationTime < DateTimeOffset.UtcNow).ToListAsync();
            queues.ForEach(q => q.IsActive = false);
            _dbcontext.UpdateRange(queues);
            await _dbcontext.SaveChangesAsync();
        }
    }
}
