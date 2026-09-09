namespace JobRecruitmentSystem.UI.Models.Employer
{
    public class CandidateProfileViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Skills { get; set; }
        public string? WorkExperience { get; set; }
        public string? CvFilePath { get; set; }
    }
}
