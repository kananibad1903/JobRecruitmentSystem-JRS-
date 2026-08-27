using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using JobRecruitmentSystem.DAL.Entities;
using JobRecruitmentSystem.DAL.Repositories.Interfaces;
using JobRecruitmentSystem.BLL.DTOs;
using JobRecruitmentSystem.BLL.Services.Interfaces;

namespace JobRecruitmentSystem.BLL.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmployerRepository _employerRepository;
        private readonly IJobSeekerRepository _jobSeekerRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IEmployerRepository employerRepository,
            IJobSeekerRepository jobSeekerRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _employerRepository = employerRepository;
            _jobSeekerRepository = jobSeekerRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                throw new Exception("Bu email artıq qeydiyyatdan keçib.");
            }

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role
            };

            await _userRepository.AddAsync(user);

            if (dto.Role == "Employer")
            {
                var employer = new Employer
                {
                    UserId = user.Id,
                    CompanyName = string.Empty,
                    CompanyDescription = string.Empty,
                    CompanyLocation = string.Empty,
                    Website = string.Empty
                };
                await _employerRepository.AddAsync(employer);
            }
            else if (dto.Role == "JobSeeker")
            {
                var jobSeeker = new JobSeeker
                {
                    UserId = user.Id,
                    Skills = string.Empty,
                    WorkExperience = string.Empty,
                    CvFilePath = string.Empty
                };
                await _jobSeekerRepository.AddAsync(jobSeeker);
            }

            var token = GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                throw new Exception("Email və ya şifrə yanlışdır.");
            }

            var token = GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }

        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}