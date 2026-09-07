using JobRecruitmentSystem.DAL.Data;
using JobRecruitmentSystem.DAL.Entities;
using JobRecruitmentSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.DAL.Repositories.Implementations
{
    public class JobPostRepository : IJobPostRepository
    {
        private readonly AppDbContext _context;

        public JobPostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JobPost> GetByIdAsync(int id)
        {
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .FirstOrDefaultAsync(_context.JobPosts.Include(jp => jp.Employer), jp => jp.Id == id);
        }

        public async Task<List<JobPost>> GetAllAsync()
        {
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .ToListAsync(_context.JobPosts.Include(jp => jp.Employer));
        }

        public async Task<List<JobPost>> GetByEmployerIdAsync(int employerId)
        {
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .ToListAsync(_context.JobPosts.Where(jp => jp.EmployerId == employerId));
        }

        public async Task<List<JobPost>> GetApprovedAsync()
        {
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .ToListAsync(_context.JobPosts.Include(jp => jp.Employer).Where(jp => jp.IsApproved == true));
        }

        public async Task<List<JobPost>> GetPendingAsync()
        {
            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                .ToListAsync(_context.JobPosts.Include(jp => jp.Employer).Where(jp => jp.IsApproved == false));
        }

        public async Task ApproveAsync(int id)
        {
            var jobPost = await _context.JobPosts.FindAsync(id);
            if (jobPost != null)
            {
                jobPost.IsApproved = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<JobPost>> SearchAsync(string category, string location, string jobType, decimal? minSalary, decimal? maxSalary)
        {
            var query = _context.JobPosts.Include(jp => jp.Employer).Where(jp => jp.IsApproved == true).AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(jp => jp.Category == category);

            if (!string.IsNullOrEmpty(location))
                query = query.Where(jp => jp.Location.Contains(location));

            if (!string.IsNullOrEmpty(jobType))
                query = query.Where(jp => jp.JobType == jobType);

            if (minSalary.HasValue)
                query = query.Where(jp => jp.SalaryMax >= minSalary.Value);

            if (maxSalary.HasValue)
                query = query.Where(jp => jp.SalaryMin <= maxSalary.Value);

            return await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(query);
        }

        public async Task AddAsync(JobPost jobPost)
        {
            await _context.JobPosts.AddAsync(jobPost);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobPost jobPost)
        {
            _context.JobPosts.Update(jobPost);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var jobPost = await _context.JobPosts.FindAsync(id);
            if (jobPost != null)
            {
                _context.JobPosts.Remove(jobPost);
                await _context.SaveChangesAsync();
            }
        }
    }
}
