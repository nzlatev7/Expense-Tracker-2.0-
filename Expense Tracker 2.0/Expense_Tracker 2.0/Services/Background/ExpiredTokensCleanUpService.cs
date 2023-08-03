using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker_2._0.Services
{
    public class ExpiredTokensCleanUpService : BackgroundService
    {
        private readonly ExpenseTrackerDbContext _dbContext;
        private readonly ILogger<ExpiredTokensCleanUpService> _logger;

        public ExpiredTokensCleanUpService(
            ExpenseTrackerDbContext dbContext,
            ILogger<ExpiredTokensCleanUpService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var allExpiredEntities = await _dbContext.ValidationTokens
                    .Where(x => x.ExpirationDate < DateTime.UtcNow)
                    .ToListAsync();

                _dbContext.RemoveRange(allExpiredEntities);

                await _dbContext.SaveChangesAsync();

                await Task.Delay(60*5000, stoppingToken);
            }
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("ExpiredTokensCleanUpService started!");

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            //here i need to dispose connections
            //but my Dependency Injection do this for me

            _logger.LogInformation("ExpiredTokensCleanUpService ended!");

            return base.StopAsync(cancellationToken);
        }
    }
}
