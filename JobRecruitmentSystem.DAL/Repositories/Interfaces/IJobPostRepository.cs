using JobRecruitmentSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.DAL.Repositories.Interfaces
{
    public interface IJobPostRepository
    {
        Task<JobPost> GetByIdAsync(int id);
        Task<List<JobPost>> GetAllAsync();
        Task<List<JobPost>> GetByEmployerIdAsync(int employerId);
        Task<List<JobPost>> GetApprovedAsync();
        Task<List<JobPost>> GetPendingAsync();
        Task ApproveAsync(int id);
        Task<List<JobPost>> SearchAsync(string category, string location, string jobType, decimal? minSalary, decimal? maxSalary);
        Task AddAsync(JobPost jobPost);
        Task UpdateAsync(JobPost jobPost);
        Task DeleteAsync(int id);
    }
}
