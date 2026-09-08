namespace JobRecruitmentSystem.UI.Models.JobPost
{
    public class JobPostListViewModel
    {
        public List<JobPostViewModel> Jobs { get; set; } = new();

        public string? Category { get; set; }
        public string? Location { get; set; }
        public string? JobType { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
    }
}
