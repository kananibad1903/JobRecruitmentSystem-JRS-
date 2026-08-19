using JobRecruitmentSystem.DAL.Data;
using JobRecruitmentSystem.DAL.Entities;
using JobRecruitmentSystem.DAL.Repositories.Interfaces;


namespace JobRecruitmentSystem.DAL.Repositories.Implementations
{
    public class JobSeekerRepository : IJobSeekerRepository
    {
        private readonly AppDbContext _context;

        public JobSeekerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JobSeeker> GetByIdAsync(int id)
        {
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .FirstOrDefaultAsync(_context.JobSeekers, js => js.Id == id);
        }

        public async Task<JobSeeker> GetByUserIdAsync(int userId)
        {
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .FirstOrDefaultAsync(_context.JobSeekers, js => js.UserId == userId);
        }

        public async Task<List<JobSeeker>> GetAllAsync()
        {
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .ToListAsync(_context.JobSeekers);
        }

        public async Task AddAsync(JobSeeker jobSeeker)
        {
            await _context.JobSeekers.AddAsync(jobSeeker);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobSeeker jobSeeker)
        {
            _context.JobSeekers.Update(jobSeeker);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var jobSeeker = await _context.JobSeekers.FindAsync(id);
            if (jobSeeker != null)
            {
                _context.JobSeekers.Remove(jobSeeker);
                await _context.SaveChangesAsync();
            }
        }
    }
}
