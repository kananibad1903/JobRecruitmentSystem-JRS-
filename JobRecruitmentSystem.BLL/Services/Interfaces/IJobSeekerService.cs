using JobRecruitmentSystem.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.Services.Interfaces
{
    public interface IJobSeekerService
    {
        Task<JobSeekerProfileDto> GetByUserIdAsync(int userId);
        Task<JobSeekerProfileDto> UpdateProfileAsync(int userId, UpdateJobSeekerProfileDto dto);
        Task<JobSeekerProfileDto> UploadCvAsync(int userId, string cvFilePath);
    }
}
