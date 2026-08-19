using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Employer Employer { get; set; }
        public JobSeeker JobSeeker { get; set; }
    }
}
