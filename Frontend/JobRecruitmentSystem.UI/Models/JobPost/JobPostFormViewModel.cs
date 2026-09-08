using System.ComponentModel.DataAnnotations;

namespace JobRecruitmentSystem.UI.Models.JobPost
{
    public class JobPostFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.MultilineText)]
        public string Requirements { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public string Location { get; set; } = string.Empty;

        [Required]
        public string JobType { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal SalaryMin { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal SalaryMax { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; } = DateTime.Today.AddMonths(1);
    }
}
