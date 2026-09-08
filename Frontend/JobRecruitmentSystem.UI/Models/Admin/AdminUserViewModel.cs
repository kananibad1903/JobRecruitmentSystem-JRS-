namespace JobRecruitmentSystem.UI.Models.Admin
{
    public class AdminUserViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
