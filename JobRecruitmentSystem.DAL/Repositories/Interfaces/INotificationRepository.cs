using JobRecruitmentSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.DAL.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetByUserIdAsync(int userId, int take = 20);
        Task<int> GetUnreadCountAsync(int userId);
        Task AddAsync(Notification notification);
        Task MarkAsReadAsync(int id, int userId);
        Task MarkAllAsReadAsync(int userId);
    }
}
