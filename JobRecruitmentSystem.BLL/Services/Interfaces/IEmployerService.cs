using JobRecruitmentSystem.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.Services.Interfaces
{
    public interface IEmployerService
    {
        Task<EmployerProfileDto> GetByUserIdAsync(int userId);
        Task<EmployerProfileDto> UpdateProfileAsync(int userId, UpdateEmployerProfileDto dto);
        Task<CandidateProfileDto> GetCandidateProfileAsync(int employerUserId, int jobSeekerId);
    }
}
