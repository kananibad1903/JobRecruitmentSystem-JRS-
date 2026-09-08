namespace JobRecruitmentSystem.UI.Models.JobPost
{
    public class JobPostDetailViewModel
    {
        public JobPostViewModel Job { get; set; } = new();
        public bool CanApply { get; set; }
        public bool CanSave { get; set; }
    }
}
