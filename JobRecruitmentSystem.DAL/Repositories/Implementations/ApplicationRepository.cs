using JobRecruitmentSystem.DAL.Data;
using JobRecruitmentSystem.DAL.Entities;

using JobRecruitmentSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JobRecruitmentSystem.DAL.Repositories.Implementations
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JobApplication> GetByIdAsync(int id)
        {
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .FirstOrDefaultAsync(
                    _context.Applications.Include(a => a.JobPost).Include(a => a.JobSeeker),
                    a => a.Id == id);
        }

        public async Task<List<JobApplication>> GetByJobSeekerIdAsync(int jobSeekerId)
        {
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .ToListAsync(_context.Applications.Include(a => a.JobPost).Where(a => a.JobSeekerId == jobSeekerId));
        }

        public async Task<List<JobApplication>> GetByJobPostIdAsync(int jobPostId)
        {
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .ToListAsync(_context.Applications.Include(a => a.JobSeeker).Where(a => a.JobPostId == jobPostId));
        }

        public async Task<bool> ExistsAsync(int jobSeekerId, int jobPostId)
        {
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .AnyAsync(_context.Applications, a => a.JobSeekerId == jobSeekerId && a.JobPostId == jobPostId);
        }

        public async Task AddAsync(JobApplication application)
        {
            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobApplication application)
        {
            _context.Applications.Update(application);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application != null)
            {
                _context.Applications.Remove(application);
                await _context.SaveChangesAsync();
            }
        }
    }
}