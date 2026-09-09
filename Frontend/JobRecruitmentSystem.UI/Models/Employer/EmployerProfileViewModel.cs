using System.ComponentModel.DataAnnotations;

namespace JobRecruitmentSystem.UI.Models.Employer
{
    public class EmployerProfileViewModel
    {
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; } = string.Empty;

        [DataType(DataType.MultilineText)]
        public string CompanyDescription { get; set; } = string.Empty;

        public string CompanyLocation { get; set; } = string.Empty;

        public string? Website { get; set; }
    }
}
