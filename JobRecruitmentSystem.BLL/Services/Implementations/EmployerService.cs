using JobRecruitmentSystem.BLL.DTOs;
using JobRecruitmentSystem.BLL.Services.Interfaces;
using JobRecruitmentSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.Services.Implementations
{
    public class EmployerService : IEmployerService
    {
        private readonly IEmployerRepository _employerRepository;

        public EmployerService(IEmployerRepository employerRepository)
        {
            _employerRepository = employerRepository;
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
            employer.Website = dto.Website;

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
    }
}
