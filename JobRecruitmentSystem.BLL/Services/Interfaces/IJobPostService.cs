using JobRecruitmentSystem.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.Services.Interfaces
{
    public interface IJobPostService
    {
        Task<JobPostDto> GetByIdAsync(int id);
        Task<List<JobPostDto>> GetAllAsync();
        Task<List<JobPostDto>> GetByEmployerUserIdAsync(int userId);
        Task<List<JobPostDto>> SearchAsync(string? category, string? location, string? jobType, decimal? minSalary, decimal? maxSalary);
        Task<List<JobPostDto>> GetPendingAsync();
        Task ApproveJobPostAsync(int jobPostId);
        Task<JobPostDto> CreateAsync(int userId, CreateJobPostDto dto);
        Task<JobPostDto> UpdateAsync(int userId, int jobPostId, UpdateJobPostDto dto);
        Task DeleteAsync(int userId, int jobPostId);
    }
}
