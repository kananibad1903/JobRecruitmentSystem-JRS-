namespace JobRecruitmentSystem.UI.Models.Application
{
    public class ApplicationViewModel
    {
        public int Id { get; set; }
        public int JobSeekerId { get; set; }
        public string? JobSeekerName { get; set; }
        public int JobPostId { get; set; }
        public string? JobPostTitle { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? EmployerNote { get; set; }
        public DateTime AppliedAt { get; set; }
    }
}
