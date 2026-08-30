using JobRecruitmentSystem.BLL.DTOs;
using JobRecruitmentSystem.BLL.Services.Interfaces;
using JobRecruitmentSystem.DAL.Entities;
using JobRecruitmentSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.Services.Implementations
{
    public class JobPostService : IJobPostService
    {
        private readonly IJobPostRepository _jobPostRepository;
        private readonly IEmployerRepository _employerRepository;

        public JobPostService(IJobPostRepository jobPostRepository, IEmployerRepository employerRepository)
        {
            _jobPostRepository = jobPostRepository;
            _employerRepository = employerRepository;
        }

        public async Task<JobPostDto> GetByIdAsync(int id)
        {
            var jobPost = await _jobPostRepository.GetByIdAsync(id);
            if (jobPost == null)
            {
                throw new Exception("Elan tapılmadı.");
            }

            return MapToDto(jobPost);
        }

        public async Task<List<JobPostDto>> GetAllAsync()
        {
            var jobPosts = await _jobPostRepository.GetAllAsync();
            return jobPosts.Select(MapToDto).ToList();
        }

        public async Task<List<JobPostDto>> GetByEmployerUserIdAsync(int userId)
        {
            var employer = await _employerRepository.GetByUserIdAsync(userId);
            if (employer == null)
            {
                throw new Exception("Employer profili tapılmadı.");
            }

            var jobPosts = await _jobPostRepository.GetByEmployerIdAsync(employer.Id);
            return jobPosts.Select(MapToDto).ToList();
        }

        public async Task<List<JobPostDto>> SearchAsync(string category, string location, string jobType, decimal? minSalary, decimal? maxSalary)
        {
            var jobPosts = await _jobPostRepository.SearchAsync(category, location, jobType, minSalary, maxSalary);
            return jobPosts.Select(MapToDto).ToList();
        }

        public async Task<JobPostDto> CreateAsync(int userId, CreateJobPostDto dto)
        {
            var employer = await _employerRepository.GetByUserIdAsync(userId);
            if (employer == null)
            {
                throw new Exception("Employer profili tapılmadı.");
            }

            var jobPost = new JobPost
            {
                EmployerId = employer.Id,
                Title = dto.Title,
                Description = dto.Description,
                Requirements = dto.Requirements,
                Category = dto.Category,
                Location = dto.Location,
                JobType = dto.JobType,
                SalaryMin = dto.SalaryMin,
                SalaryMax = dto.SalaryMax,
                Deadline = dto.Deadline
            };

            await _jobPostRepository.AddAsync(jobPost);

            jobPost.Employer = employer;
            return MapToDto(jobPost);
        }

        public async Task<JobPostDto> UpdateAsync(int userId, int jobPostId, UpdateJobPostDto dto)
        {
            var employer = await _employerRepository.GetByUserIdAsync(userId);
            if (employer == null)
            {
                throw new Exception("Employer profili tapılmadı.");
            }

            var jobPost = await _jobPostRepository.GetByIdAsync(jobPostId);
            if (jobPost == null)
            {
                throw new Exception("Elan tapılmadı.");
            }

            if (jobPost.EmployerId != employer.Id)
            {
                throw new Exception("Bu elanı redaktə etmək icazəniz yoxdur.");
            }

            jobPost.Title = dto.Title;
            jobPost.Description = dto.Description;
            jobPost.Requirements = dto.Requirements;
            jobPost.Category = dto.Category;
            jobPost.Location = dto.Location;
            jobPost.JobType = dto.JobType;
            jobPost.SalaryMin = dto.SalaryMin;
            jobPost.SalaryMax = dto.SalaryMax;
            jobPost.Deadline = dto.Deadline;

            await _jobPostRepository.UpdateAsync(jobPost);

            return MapToDto(jobPost);
        }

        public async Task DeleteAsync(int userId, int jobPostId)
        {
            var employer = await _employerRepository.GetByUserIdAsync(userId);
            if (employer == null)
            {
                throw new Exception("Employer profili tapılmadı.");
            }

            var jobPost = await _jobPostRepository.GetByIdAsync(jobPostId);
            if (jobPost == null)
            {
                throw new Exception("Elan tapılmadı.");
            }

            if (jobPost.EmployerId != employer.Id)
            {
                throw new Exception("Bu elanı silmək icazəniz yoxdur.");
            }

            await _jobPostRepository.DeleteAsync(jobPostId);
        }

        private JobPostDto MapToDto(JobPost jobPost)
        {
            return new JobPostDto
            {
                Id = jobPost.Id,
                EmployerId = jobPost.EmployerId,
                CompanyName = jobPost.Employer?.CompanyName,
                Title = jobPost.Title,
                Description = jobPost.Description,
                Requirements = jobPost.Requirements,
                Category = jobPost.Category,
                Location = jobPost.Location,
                JobType = jobPost.JobType,
                SalaryMin = jobPost.SalaryMin,
                SalaryMax = jobPost.SalaryMax,
                Deadline = jobPost.Deadline,
                CreatedAt = jobPost.CreatedAt
            };
        }
    }
}
