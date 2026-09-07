using JobRecruitmentSystem.BLL.DTOs;
using JobRecruitmentSystem.BLL.Services.Interfaces;
using JobRecruitmentSystem.DAL.Entities;
using JobRecruitmentSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJobPostRepository _jobPostRepository;

        public AdminService(IUserRepository userRepository, IJobPostRepository jobPostRepository)
        {
            _userRepository = userRepository;
            _jobPostRepository = jobPostRepository;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = u.Role,
                EmailConfirmed = u.EmailConfirmed,
                CreatedAt = u.CreatedAt
            }).ToList();
        }

        public async Task DeleteUserAsync(int userId)
        {
            await _userRepository.DeleteAsync(userId);
        }

        public async Task<List<JobPostDto>> GetAllJobPostsAsync()
        {
            var jobPosts = await _jobPostRepository.GetAllAsync();
            return jobPosts.Select(MapToDto).ToList();
        }

        public async Task<List<JobPostDto>> GetPendingJobPostsAsync()
        {
            var jobPosts = await _jobPostRepository.GetPendingAsync();
            return jobPosts.Select(MapToDto).ToList();
        }

        public async Task ApproveJobPostAsync(int jobPostId)
        {
            await _jobPostRepository.ApproveAsync(jobPostId);
        }

        public async Task DeleteJobPostAsync(int jobPostId)
        {
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
                CreatedAt = jobPost.CreatedAt,
                IsApproved = jobPost.IsApproved
            };
        }
    }
}
