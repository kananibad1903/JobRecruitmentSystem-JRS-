using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.DTOs
{
    public class UpdateEmployerProfileDto
    {
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string CompanyLocation { get; set; }
        public string? Website { get; set; }
    }
}
