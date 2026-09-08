using System.ComponentModel.DataAnnotations;

namespace JobRecruitmentSystem.UI.Models.Application
{
    public class UpdateApplicationStatusViewModel
    {
        public int Id { get; set; }
        public int JobPostId { get; set; }

        [Required]
        public string Status { get; set; } = "Applied";

        public string? EmployerNote { get; set; }
    }
}
