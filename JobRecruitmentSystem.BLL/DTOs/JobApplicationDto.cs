using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.DTOs
{
    public class JobApplicationDto
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public string JobSeekerName { get; set; }
        public int JobPostId { get; set; }
        public string JobPostTitle { get; set; }
        public string Status { get; set; }
        public string EmployerNote { get; set; }
        public DateTime AppliedAt { get; set; }
    }
}
