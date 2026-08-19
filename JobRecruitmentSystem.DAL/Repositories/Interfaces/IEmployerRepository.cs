using JobRecruitmentSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.DAL.Repositories.Interfaces
{
    public interface IEmployerRepository
    {
        Task<Employer> GetByIdAsync(int id);
        Task<Employer> GetByUserIdAsync(int userId);
        Task<List<Employer>> GetAllAsync();
        Task AddAsync(Employer employer);
        Task UpdateAsync(Employer employer);
        Task DeleteAsync(int id);
    }
}
