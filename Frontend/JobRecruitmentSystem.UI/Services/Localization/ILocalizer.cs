namespace JobRecruitmentSystem.UI.Services.Localization
{
    public interface ILocalizer
    {
        string T(string key);

        string CurrentCulture { get; }
    }
}
