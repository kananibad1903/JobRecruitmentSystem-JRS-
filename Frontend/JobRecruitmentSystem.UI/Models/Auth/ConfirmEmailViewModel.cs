using System.ComponentModel.DataAnnotations;

namespace JobRecruitmentSystem.UI.Models.Auth
{
    public class ConfirmEmailViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Code { get; set; } = string.Empty;
    }
}
