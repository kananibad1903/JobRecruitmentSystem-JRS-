using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.DAL.Entities
{
    public class JobApplication
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public JobSeeker JobSeeker { get; set; }
        public int JobPostId { get; set; }
        public JobPost JobPost { get; set; }

        public string Status { get; set; } = "Applied";
        public string EmployerNote { get; set; }
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    }
}
