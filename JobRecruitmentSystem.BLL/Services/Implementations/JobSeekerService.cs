using JobRecruitmentSystem.BLL.DTOs;
using JobRecruitmentSystem.BLL.Services.Interfaces;
using JobRecruitmentSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.Services.Implementations
{
    public class JobSeekerService : IJobSeekerService
    {
        private readonly IJobSeekerRepository _jobSeekerRepository;

        public JobSeekerService(IJobSeekerRepository jobSeekerRepository)
        {
            _jobSeekerRepository = jobSeekerRepository;
        }

        public async Task<JobSeekerProfileDto> GetByUserIdAsync(int userId)
        {
            var jobSeeker = await _jobSeekerRepository.GetByUserIdAsync(userId);
            if (jobSeeker == null)
            {
                throw new Exception("Job seeker profili tapılmadı.");
            }

            return new JobSeekerProfileDto
            {
                Id = jobSeeker.Id,
                Skills = jobSeeker.Skills,
                WorkExperience = jobSeeker.WorkExperience,
                CvFilePath = jobSeeker.CvFilePath
            };
        }

        public async Task<JobSeekerProfileDto> UpdateProfileAsync(int userId, UpdateJobSeekerProfileDto dto)
        {
            var jobSeeker = await _jobSeekerRepository.GetByUserIdAsync(userId);
            if (jobSeeker == null)
            {
                throw new Exception("Job seeker profili tapılmadı.");
            }

            jobSeeker.Skills = dto.Skills;
            jobSeeker.WorkExperience = dto.WorkExperience;

            await _jobSeekerRepository.UpdateAsync(jobSeeker);

            return new JobSeekerProfileDto
            {
                Id = jobSeeker.Id,
                Skills = jobSeeker.Skills,
                WorkExperience = jobSeeker.WorkExperience,
                CvFilePath = jobSeeker.CvFilePath
            };
        }

        public async Task<JobSeekerProfileDto> UploadCvAsync(int userId, string cvFilePath)
        {
            var jobSeeker = await _jobSeekerRepository.GetByUserIdAsync(userId);
            if (jobSeeker == null)
            {
                throw new Exception("Job seeker profili tapılmadı.");
            }

            jobSeeker.CvFilePath = cvFilePath;
            await _jobSeekerRepository.UpdateAsync(jobSeeker);

            return new JobSeekerProfileDto
            {
                Id = jobSeeker.Id,
                Skills = jobSeeker.Skills,
                WorkExperience = jobSeeker.WorkExperience,
                CvFilePath = jobSeeker.CvFilePath
            };
        }
    }
}
