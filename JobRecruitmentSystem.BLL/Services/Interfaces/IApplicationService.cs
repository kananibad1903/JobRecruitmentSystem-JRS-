using JobRecruitmentSystem.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<JobApplicationDto> ApplyAsync(int userId, CreateApplicationDto dto);
        Task<List<JobApplicationDto>> GetMyApplicationsAsync(int userId);
        Task<List<JobApplicationDto>> GetApplicationsForJobPostAsync(int userId, int jobPostId);
        Task<JobApplicationDto> UpdateStatusAsync(int userId, int applicationId, UpdateApplicationStatusDto dto);
    }
}
