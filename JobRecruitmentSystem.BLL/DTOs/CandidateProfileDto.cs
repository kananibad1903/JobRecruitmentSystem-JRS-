using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.DTOs
{
    public class CandidateProfileDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Skills { get; set; }
        public string WorkExperience { get; set; }
        public string CvFilePath { get; set; }
    }
}
