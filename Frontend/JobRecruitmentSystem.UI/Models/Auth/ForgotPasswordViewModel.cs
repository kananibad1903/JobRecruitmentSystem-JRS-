using System.ComponentModel.DataAnnotations;

namespace JobRecruitmentSystem.UI.Models.Auth
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
