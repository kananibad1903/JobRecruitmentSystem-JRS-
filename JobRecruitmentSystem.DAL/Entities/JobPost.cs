using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace JobRecruitmentSystem.DAL.Entities
{
    public class JobPost
    {
        public int Id { get; set; }
        public int EmployerId { get; set; }
        public Employer Employer { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public string JobType { get; set; }
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsApproved { get; set; } = false;
        public bool IsExpired { get; set; } = false;

        public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
        public ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();
    }
}
