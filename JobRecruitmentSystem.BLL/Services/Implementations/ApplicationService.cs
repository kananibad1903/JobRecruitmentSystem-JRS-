using JobRecruitmentSystem.BLL.DTOs;
using JobRecruitmentSystem.BLL.Services.Interfaces;
using JobRecruitmentSystem.DAL.Entities;
using JobRecruitmentSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.Services.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IJobSeekerRepository _jobSeekerRepository;
        private readonly IEmployerRepository _employerRepository;
        private readonly IJobPostRepository _jobPostRepository;

        public ApplicationService(
            IApplicationRepository applicationRepository,
            IJobSeekerRepository jobSeekerRepository,
            IEmployerRepository employerRepository,
            IJobPostRepository jobPostRepository)
        {
            _applicationRepository = applicationRepository;
            _jobSeekerRepository = jobSeekerRepository;
            _employerRepository = employerRepository;
            _jobPostRepository = jobPostRepository;
        }

        public async Task<JobApplicationDto> ApplyAsync(int userId, CreateApplicationDto dto)
        {
            var jobSeeker = await _jobSeekerRepository.GetByUserIdAsync(userId);
            if (jobSeeker == null)
            {
                throw new Exception("Job seeker profili tapılmadı.");
            }

            var jobPost = await _jobPostRepository.GetByIdAsync(dto.JobPostId);
            if (jobPost == null)
            {
                throw new Exception("Elan tapılmadı.");
            }

            var alreadyApplied = await _applicationRepository.ExistsAsync(jobSeeker.Id, dto.JobPostId);
            if (alreadyApplied)
            {
                throw new Exception("Bu elana artıq müraciət etmisiniz.");
            }

            var application = new JobApplication
            {
                JobSeekerId = jobSeeker.Id,
                JobPostId = dto.JobPostId,
                Status = "Applied",
                EmployerNote = string.Empty
            };

            await _applicationRepository.AddAsync(application);

            return new JobApplicationDto
            {
                Id = application.Id,
                JobSeekerId = jobSeeker.Id,
                JobPostId = jobPost.Id,
                JobPostTitle = jobPost.Title,
                Status = application.Status,
                EmployerNote = application.EmployerNote,
                AppliedAt = application.AppliedAt
            };
        }

        public async Task<List<JobApplicationDto>> GetMyApplicationsAsync(int userId)
        {
            var jobSeeker = await _jobSeekerRepository.GetByUserIdAsync(userId);
            if (jobSeeker == null)
            {
                throw new Exception("Job seeker profili tapılmadı.");
            }

            var applications = await _applicationRepository.GetByJobSeekerIdAsync(jobSeeker.Id);

            return applications.Select(a => new JobApplicationDto
            {
                Id = a.Id,
                JobSeekerId = a.JobSeekerId,
                JobPostId = a.JobPostId,
                JobPostTitle = a.JobPost?.Title,
                Status = a.Status,
                EmployerNote = a.EmployerNote,
                AppliedAt = a.AppliedAt
            }).ToList();
        }

        public async Task<List<JobApplicationDto>> GetApplicationsForJobPostAsync(int userId, int jobPostId)
        {
            var employer = await _employerRepository.GetByUserIdAsync(userId);
            if (employer == null)
            {
                throw new Exception("Employer profili tapılmadı.");
            }

            var jobPost = await _jobPostRepository.GetByIdAsync(jobPostId);
            if (jobPost == null || jobPost.EmployerId != employer.Id)
            {
                throw new Exception("Bu elana aid müraciətlərə baxmaq icazəniz yoxdur.");
            }

            var applications = await _applicationRepository.GetByJobPostIdAsync(jobPostId);

            return applications.Select(a => new JobApplicationDto
            {
                Id = a.Id,
                JobSeekerId = a.JobSeekerId,
                JobSeekerName = a.JobSeeker?.User?.FullName,
                JobPostId = a.JobPostId,
                Status = a.Status,
                EmployerNote = a.EmployerNote,
                AppliedAt = a.AppliedAt
            }).ToList();
        }

        public async Task<JobApplicationDto> UpdateStatusAsync(int userId, int applicationId, UpdateApplicationStatusDto dto)
        {
            var employer = await _employerRepository.GetByUserIdAsync(userId);
            if (employer == null)
            {
                throw new Exception("Employer profili tapılmadı.");
            }

            var application = await _applicationRepository.GetByIdAsync(applicationId);
            if (application == null)
            {
                throw new Exception("Müraciət tapılmadı.");
            }

            if (application.JobPost.EmployerId != employer.Id)
            {
                throw new Exception("Bu müraciəti dəyişmək icazəniz yoxdur.");
            }

            application.Status = dto.Status;
            application.EmployerNote = dto.EmployerNote;

            await _applicationRepository.UpdateAsync(application);

            return new JobApplicationDto
            {
                Id = application.Id,
                JobSeekerId = application.JobSeekerId,
                JobPostId = application.JobPostId,
                JobPostTitle = application.JobPost?.Title,
                Status = application.Status,
                EmployerNote = application.EmployerNote,
                AppliedAt = application.AppliedAt
            };
        }
    }
}
