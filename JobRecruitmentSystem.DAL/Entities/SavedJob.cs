using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.DAL.Entities
{
    public class SavedJob
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public JobSeeker JobSeeker { get; set; }
        public int JobPostId { get; set; }
        public JobPost JobPost { get; set; }
        public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    }
}
