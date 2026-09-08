using System.Text.Json;

namespace JobRecruitmentSystem.UI.Services.Localization
{
    public class JsonLocalizer : ILocalizer
    {
        public const string CookieName = "ui_lang";
        public const string DefaultCulture = "az";
        public static readonly string[] SupportedCultures = { "az", "en", "ru", "tr" };

        private static readonly Dictionary<string, Dictionary<string, string>> Dictionaries = Load();

        private readonly IHttpContextAccessor _accessor;

        public JsonLocalizer(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string CurrentCulture
        {
            get
            {
                var cookie = _accessor.HttpContext?.Request.Cookies[CookieName];
                return cookie is not null && SupportedCultures.Contains(cookie) ? cookie : DefaultCulture;
            }
        }

        public string T(string key)
        {
            var culture = CurrentCulture;

            if (Dictionaries.TryGetValue(culture, out var map) && map.TryGetValue(key, out var value))
            {
                return value;
            }

            if (Dictionaries.TryGetValue(DefaultCulture, out var fallbackMap) && fallbackMap.TryGetValue(key, out var fallbackValue))
            {
                return fallbackValue;
            }

            return key;
        }

        private static Dictionary<string, Dictionary<string, string>> Load()
        {
            var result = new Dictionary<string, Dictionary<string, string>>();
            var resourcesDir = Path.Combine(AppContext.BaseDirectory, "Resources", "lang");

            foreach (var culture in SupportedCultures)
            {
                var path = Path.Combine(resourcesDir, $"{culture}.json");
                if (File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    result[culture] = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
                }
                else
                {
                    result[culture] = new Dictionary<string, string>();
                }
            }

            return result;
        }
    }
}
