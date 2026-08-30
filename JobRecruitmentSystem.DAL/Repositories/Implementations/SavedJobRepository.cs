using JobRecruitmentSystem.DAL.Data;
using JobRecruitmentSystem.DAL.Entities;
using JobRecruitmentSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace JobRecruitmentSystem.DAL.Repositories.Implementations
{
    public class SavedJobRepository : ISavedJobRepository
    {
        private readonly AppDbContext _context;

        public SavedJobRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SavedJob>> GetByJobSeekerIdAsync(int jobSeekerId)
        {
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .ToListAsync(_context.SavedJobs.Include(sj => sj.JobPost).Where(sj => sj.JobSeekerId == jobSeekerId));
        }

        public async Task<bool> ExistsAsync(int jobSeekerId, int jobPostId)
        {
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .AnyAsync(_context.SavedJobs, sj => sj.JobSeekerId == jobSeekerId && sj.JobPostId == jobPostId);
        }

        public async Task AddAsync(SavedJob savedJob)
        {
            await _context.SavedJobs.AddAsync(savedJob);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var savedJob = await _context.SavedJobs.FindAsync(id);
            if (savedJob != null)
            {
                _context.SavedJobs.Remove(savedJob);
                await _context.SaveChangesAsync();
            }
        }
    }
}
