namespace JobRecruitmentSystem.UI.Models.JobSeeker
{
    public class SavedJobViewModel
    {
        public int Id { get; set; }
        public int JobPostId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? CompanyName { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime SavedAt { get; set; }
    }
}
