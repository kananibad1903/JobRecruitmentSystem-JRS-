using JobRecruitmentSystem.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.Services.Interfaces
{
    public interface ISavedJobService
    {
        Task<SavedJobDto> SaveAsync(int userId, int jobPostId);
        Task<List<SavedJobDto>> GetMySavedJobsAsync(int userId);
        Task RemoveAsync(int userId, int jobPostId);
    }
}
