using System.ComponentModel.DataAnnotations;

namespace JobRecruitmentSystem.UI.Models.JobSeeker
{
    public class JobSeekerProfileViewModel
    {
        public int Id { get; set; }

        [DataType(DataType.MultilineText)]
        public string Skills { get; set; } = string.Empty;

        [DataType(DataType.MultilineText)]
        public string WorkExperience { get; set; } = string.Empty;

        public string? CvFilePath { get; set; }
    }
}
