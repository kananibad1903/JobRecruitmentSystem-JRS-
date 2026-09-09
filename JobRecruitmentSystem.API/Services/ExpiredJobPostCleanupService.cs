using JobRecruitmentSystem.BLL.Services.Interfaces;
using JobRecruitmentSystem.DAL.Repositories.Interfaces;

namespace JobRecruitmentSystem.API.Services
{
    /// <summary>
    /// Periodically closes job posts whose deadline has passed: flags them as
    /// expired (hidden from public listings, kept intact for the employer)
    /// and notifies the employer. Runs once at startup, then hourly.
    /// </summary>
    public class ExpiredJobPostCleanupService : BackgroundService
    {
        private static readonly TimeSpan Interval = TimeSpan.FromHours(1);

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ExpiredJobPostCleanupService> _logger;

        public ExpiredJobPostCleanupService(IServiceScopeFactory scopeFactory, ILogger<ExpiredJobPostCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(Interval);

            do
            {
                try
                {
                    await CloseExpiredJobPostsAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to close expired job posts.");
                }
            } while (await timer.WaitForNextTickAsync(stoppingToken));
        }

        private async Task CloseExpiredJobPostsAsync(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var jobPostRepository = scope.ServiceProvider.GetRequiredService<IJobPostRepository>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            var expiring = await jobPostRepository.GetExpiringAsync();

            foreach (var jobPost in expiring)
            {
                await notificationService.CreateAsync(
                    jobPost.Employer.UserId,
                    $"'{jobPost.Title}' elanının müddəti bitdi və artıq aktiv deyil.",
                    "/Employer/MyPosts");

                await jobPostRepository.MarkExpiredAsync(jobPost.Id);
            }
        }
    }
}
