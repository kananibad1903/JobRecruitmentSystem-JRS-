using JobRecruitmentSystem.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.Services.Interfaces
{
    public interface IAdminService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task DeleteUserAsync(int userId);
        Task<List<JobPostDto>> GetAllJobPostsAsync();
        Task<List<JobPostDto>> GetPendingJobPostsAsync();
        Task ApproveJobPostAsync(int jobPostId);
        Task DeleteJobPostAsync(int jobPostId);
    }
}
