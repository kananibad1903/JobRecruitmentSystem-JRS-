using JobRecruitmentSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.DAL.Repositories.Interfaces
{
    public interface IApplicationRepository
    {
        Task<JobApplication> GetByIdAsync(int id);
        Task<List<JobApplication>> GetByJobSeekerIdAsync(int jobSeekerId);
        Task<List<JobApplication>> GetByJobPostIdAsync(int jobPostId);
        Task<bool> ExistsAsync(int jobSeekerId, int jobPostId);
        Task AddAsync(JobApplication application);
        Task UpdateAsync(JobApplication application);
        Task DeleteAsync(int id);
    }
}
