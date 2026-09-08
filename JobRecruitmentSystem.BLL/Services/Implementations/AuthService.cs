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
        private readonly IEmailService _emailService;

        public AuthService(
            IUserRepository userRepository,
            IEmployerRepository employerRepository,
            IJobSeekerRepository jobSeekerRepository,
            IConfiguration configuration,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _employerRepository = employerRepository;
            _jobSeekerRepository = jobSeekerRepository;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                throw new Exception("Bu email artıq qeydiyyatdan keçib.");
            }

            var code = new Random().Next(100000, 999999).ToString();

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                EmailConfirmed = false,
                EmailConfirmationToken = code,
                PasswordResetCode = string.Empty
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

            await _emailService.SendConfirmationCodeAsync(user.Email, code);

            return new AuthResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                Token = null
            };
        }

        public async Task ConfirmEmailAsync(ConfirmEmailDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
            {
                throw new Exception("İstifadəçi tapılmadı.");
            }

            if (user.EmailConfirmed)
            {
                throw new Exception("Email artıq təsdiqlənib.");
            }

            if (user.EmailConfirmationToken != dto.Code)
            {
                throw new Exception("Kod yanlışdır.");
            }

            user.EmailConfirmed = true;
            user.EmailConfirmationToken = string.Empty;
            await _userRepository.UpdateAsync(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                throw new Exception("Email və ya şifrə yanlışdır.");
            }

            if (!user.EmailConfirmed)
            {
                throw new Exception("Zəhmət olmasa əvvəlcə email ünvanınızı təsdiqləyin.");
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
        public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
            {
                throw new Exception("Bu email ilə istifadəçi tapılmadı.");
            }

            var code = new Random().Next(100000, 999999).ToString();
            user.PasswordResetCode = code;
            await _userRepository.UpdateAsync(user);

            await _emailService.SendPasswordResetCodeAsync(user.Email, code);
        }

        public async Task ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
            {
                throw new Exception("İstifadəçi tapılmadı.");
            }

            if (string.IsNullOrEmpty(user.PasswordResetCode) || user.PasswordResetCode != dto.Code)
            {
                throw new Exception("Kod yanlışdır.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.PasswordResetCode = string.Empty;
            await _userRepository.UpdateAsync(user);
        }
    }
}