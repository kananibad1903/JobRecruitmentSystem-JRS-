using JobRecruitmentSystem.BLL.DTOs;
using JobRecruitmentSystem.BLL.Services.Interfaces;
using JobRecruitmentSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobRecruitmentSystem.BLL.Services.Implementations
{
    public class EmployerService : IEmployerService
    {
        private readonly IEmployerRepository _employerRepository;
        private readonly IJobPostRepository _jobPostRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IJobSeekerRepository _jobSeekerRepository;

        public EmployerService(
            IEmployerRepository employerRepository,
            IJobPostRepository jobPostRepository,
            IApplicationRepository applicationRepository,
            IJobSeekerRepository jobSeekerRepository)
        {
            _employerRepository = employerRepository;
            _jobPostRepository = jobPostRepository;
            _applicationRepository = applicationRepository;
            _jobSeekerRepository = jobSeekerRepository;
        }

        public async Task<EmployerProfileDto> GetByUserIdAsync(int userId)
        {
            var employer = await _employerRepository.GetByUserIdAsync(userId);
            if (employer == null)
            {
                throw new Exception("Employer profili tapılmadı.");
            }

            return new EmployerProfileDto
            {
                Id = employer.Id,
                CompanyName = employer.CompanyName,
                CompanyDescription = employer.CompanyDescription,
                CompanyLocation = employer.CompanyLocation,
                Website = employer.Website
            };
        }

        public async Task<EmployerProfileDto> UpdateProfileAsync(int userId, UpdateEmployerProfileDto dto)
        {
            var employer = await _employerRepository.GetByUserIdAsync(userId);
            if (employer == null)
            {
                throw new Exception("Employer profili tapılmadı.");
            }

            employer.CompanyName = dto.CompanyName;
            employer.CompanyDescription = dto.CompanyDescription;
            employer.CompanyLocation = dto.CompanyLocation;
            employer.Website = dto.Website ?? string.Empty;

            await _employerRepository.UpdateAsync(employer);

            return new EmployerProfileDto
            {
                Id = employer.Id,
                CompanyName = employer.CompanyName,
                CompanyDescription = employer.CompanyDescription,
                CompanyLocation = employer.CompanyLocation,
                Website = employer.Website
            };
        }

        public async Task<CandidateProfileDto> GetCandidateProfileAsync(int employerUserId, int jobSeekerId)
        {
            var employer = await _employerRepository.GetByUserIdAsync(employerUserId);
            if (employer == null)
            {
                throw new Exception("Employer profili tapılmadı.");
            }

            var employerPostIds = (await _jobPostRepository.GetByEmployerIdAsync(employer.Id))
                .Select(jp => jp.Id)
                .ToHashSet();

            var candidateApplications = await _applicationRepository.GetByJobSeekerIdAsync(jobSeekerId);
            var hasAppliedToEmployer = candidateApplications.Any(a => employerPostIds.Contains(a.JobPostId));

            if (!hasAppliedToEmployer)
            {
                throw new Exception("Bu namizədin profilinə baxmaq icazəniz yoxdur.");
            }

            var jobSeeker = await _jobSeekerRepository.GetByIdWithUserAsync(jobSeekerId);
            if (jobSeeker == null)
            {
                throw new Exception("Namizəd tapılmadı.");
            }

            return new CandidateProfileDto
            {
                Id = jobSeeker.Id,
                FullName = jobSeeker.User?.FullName,
                Email = jobSeeker.User?.Email,
                Skills = jobSeeker.Skills,
                WorkExperience = jobSeeker.WorkExperience,
                CvFilePath = jobSeeker.CvFilePath
            };
        }
    }
}
