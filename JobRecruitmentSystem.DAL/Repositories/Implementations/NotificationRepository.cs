using JobRecruitmentSystem.DAL.Data;
using JobRecruitmentSystem.DAL.Entities;
using JobRecruitmentSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobRecruitmentSystem.DAL.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetByUserIdAsync(int userId, int take = 20)
        {
            var query = _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(take);

            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(query);
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .CountAsync(_context.Notifications, n => n.UserId == userId && n.IsRead == false);
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(int id, int userId)
        {
            var notification = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .FirstOrDefaultAsync(_context.Notifications, n => n.Id == id && n.UserId == userId);

            if (notification != null && !notification.IsRead)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var unread = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .ToListAsync(_context.Notifications.Where(n => n.UserId == userId && n.IsRead == false));

            foreach (var notification in unread)
            {
                notification.IsRead = true;
            }

            if (unread.Count > 0)
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}
