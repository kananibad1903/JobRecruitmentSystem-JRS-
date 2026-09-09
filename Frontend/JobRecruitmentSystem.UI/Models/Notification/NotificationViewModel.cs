namespace JobRecruitmentSystem.UI.Models.Notification
{
    public class NotificationViewModel
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Link { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
