using JobRecruitmentSystem.BLL.DTOs;
using System.Collections.Generic;

namespace JobRecruitmentSystem.BLL.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetMyNotificationsAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task CreateAsync(int userId, string message, string? link = null);
        Task MarkAsReadAsync(int id, int userId);
        Task MarkAllAsReadAsync(int userId);
    }
}
