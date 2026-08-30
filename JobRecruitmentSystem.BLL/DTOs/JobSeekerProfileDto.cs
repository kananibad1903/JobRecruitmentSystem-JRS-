using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.DTOs
{
    public class JobSeekerProfileDto
    {
        public int Id { get; set; }
        public string Skills { get; set; }
        public string WorkExperience { get; set; }
        public string CvFilePath { get; set; }
    }
}
