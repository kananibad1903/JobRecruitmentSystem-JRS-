using JobRecruitmentSystem.BLL.DTOs;
using JobRecruitmentSystem.BLL.Services.Interfaces;
using JobRecruitmentSystem.DAL.Entities;
using JobRecruitmentSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.Services.Implementations
{
    public class SavedJobService : ISavedJobService
    {
        private readonly ISavedJobRepository _savedJobRepository;
        private readonly IJobSeekerRepository _jobSeekerRepository;
        private readonly IJobPostRepository _jobPostRepository;

        public SavedJobService(
            ISavedJobRepository savedJobRepository,
            IJobSeekerRepository jobSeekerRepository,
            IJobPostRepository jobPostRepository)
        {
            _savedJobRepository = savedJobRepository;
            _jobSeekerRepository = jobSeekerRepository;
            _jobPostRepository = jobPostRepository;
        }

        public async Task<SavedJobDto> SaveAsync(int userId, int jobPostId)
        {
            var jobSeeker = await _jobSeekerRepository.GetByUserIdAsync(userId);
            if (jobSeeker == null)
            {
                throw new Exception("Job seeker profili tapılmadı.");
            }

            var jobPost = await _jobPostRepository.GetByIdAsync(jobPostId);
            if (jobPost == null)
            {
                throw new Exception("Elan tapılmadı.");
            }

            var alreadySaved = await _savedJobRepository.ExistsAsync(jobSeeker.Id, jobPostId);
            if (alreadySaved)
            {
                throw new Exception("Bu elan artıq saxlanılıb.");
            }

            var savedJob = new SavedJob
            {
                JobSeekerId = jobSeeker.Id,
                JobPostId = jobPostId
            };

            await _savedJobRepository.AddAsync(savedJob);

            return new SavedJobDto
            {
                Id = savedJob.Id,
                JobPostId = jobPost.Id,
                Title = jobPost.Title,
                CompanyName = jobPost.Employer?.CompanyName,
                Location = jobPost.Location,
                SavedAt = savedJob.SavedAt
            };
        }

        public async Task<List<SavedJobDto>> GetMySavedJobsAsync(int userId)
        {
            var jobSeeker = await _jobSeekerRepository.GetByUserIdAsync(userId);
            if (jobSeeker == null)
            {
                throw new Exception("Job seeker profili tapılmadı.");
            }

            var savedJobs = await _savedJobRepository.GetByJobSeekerIdAsync(jobSeeker.Id);

            return savedJobs.Select(sj => new SavedJobDto
            {
                Id = sj.Id,
                JobPostId = sj.JobPostId,
                Title = sj.JobPost?.Title,
                CompanyName = sj.JobPost?.Employer?.CompanyName,
                Location = sj.JobPost?.Location,
                SavedAt = sj.SavedAt
            }).ToList();
        }

        public async Task RemoveAsync(int userId, int jobPostId)
        {
            var jobSeeker = await _jobSeekerRepository.GetByUserIdAsync(userId);
            if (jobSeeker == null)
            {
                throw new Exception("Job seeker profili tapılmadı.");
            }

            var savedJobs = await _savedJobRepository.GetByJobSeekerIdAsync(jobSeeker.Id);
            var savedJob = savedJobs.FirstOrDefault(sj => sj.JobPostId == jobPostId);

            if (savedJob == null)
            {
                throw new Exception("Saxlanılan elan tapılmadı.");
            }

            await _savedJobRepository.DeleteAsync(savedJob.Id);
        }
    }
}
