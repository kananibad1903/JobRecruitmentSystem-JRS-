using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.DTOs
{
    public class RegisterDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
