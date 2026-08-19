using JobRecruitmentSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.DAL.Repositories.Interfaces
{
    public interface IJobSeekerRepository
    {
        Task<JobSeeker> GetByIdAsync(int id);
        Task<JobSeeker> GetByUserIdAsync(int userId);
        Task<List<JobSeeker>> GetAllAsync();
        Task AddAsync(JobSeeker jobSeeker);
        Task UpdateAsync(JobSeeker jobSeeker);
        Task DeleteAsync(int id);
    }
}
