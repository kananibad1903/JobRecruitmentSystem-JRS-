using JobRecruitmentSystem.DAL.Data;
using JobRecruitmentSystem.DAL.Entities;
using JobRecruitmentSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.DAL.Repositories.Implementations
{
    public class EmployerRepository : IEmployerRepository
    {
        private readonly AppDbContext _context;

        public EmployerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Employer> GetByIdAsync(int id)
        {
            return await _context.Employers.Include(e => e.JobPosts).FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Employer> GetByUserIdAsync(int userId)
        {
            return await _context.Employers.FirstOrDefaultAsync(e => e.UserId == userId);
        }

        public async Task<List<Employer>> GetAllAsync()
        {
            return await _context.Employers.ToListAsync();
        }

        public async Task AddAsync(Employer employer)
        {
            await _context.Employers.AddAsync(employer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employer employer)
        {
            _context.Employers.Update(employer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employer = await _context.Employers.FindAsync(id);
            if (employer != null)
            {
                _context.Employers.Remove(employer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
