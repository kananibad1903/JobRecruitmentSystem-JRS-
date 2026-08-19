using JobRecruitmentSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.DAL.Repositories.Interfaces
{
    public interface ISavedJobRepository
    {
        Task<List<SavedJob>> GetByJobSeekerIdAsync(int jobSeekerId);
        Task<bool> ExistsAsync(int jobSeekerId, int jobPostId);
        Task AddAsync(SavedJob savedJob);
        Task DeleteAsync(int id);
    }
}
