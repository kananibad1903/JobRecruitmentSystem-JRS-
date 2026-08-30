using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.DTOs
{
    public class SavedJobDto
    {
        public int Id { get; set; }
        public int JobPostId { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public DateTime SavedAt { get; set; }
    }
}
