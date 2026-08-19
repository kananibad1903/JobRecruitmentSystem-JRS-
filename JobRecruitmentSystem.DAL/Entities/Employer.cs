using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.DAL.Entities
{
    public class Employer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string CompanyLocation { get; set; }
        public string Website { get; set; }

        public ICollection<JobPost> JobPosts { get; set; } = new List<JobPost>();
    }
}
