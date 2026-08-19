using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace JobRecruitmentSystem.DAL.Entities
{
    public class JobSeeker
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public string Skills { get; set; }
        public string WorkExperience { get; set; }
        public string CvFilePath { get; set; }

        public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
        public ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();
    }
}
