namespace Expense_Tracker_2._0.Services
{
    public class ExpiredTokensCleanupService : IHostedService, IDisposable
        //: BackgroundService
    {
        private Timer _timer;
        private readonly ExpenseTrackerDbContext _dbContext;

        public ExpiredTokensCleanupService(ExpenseTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DeleteExpiredTokens, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        //Delete all expiredTokens in every 5min
        public void DeleteExpiredTokens(object state)
        {
            var expiredTokens = _dbContext.ValidationTokens
                .Where(x => x.ExpirationDate < DateTime.UtcNow);

            foreach (var expiredToken in expiredTokens)
            { 
                _dbContext.ValidationTokens.Remove(expiredToken);
            }

            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
