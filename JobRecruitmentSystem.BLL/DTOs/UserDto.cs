using System;
using System.Collections.Generic;
using System.Text;

namespace JobRecruitmentSystem.BLL.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
