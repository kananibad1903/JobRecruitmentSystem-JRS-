using JobRecruitmentSystem.UI.Services.Localization;
using Microsoft.AspNetCore.Mvc;

namespace JobRecruitmentSystem.UI.Controllers
{
    public class LanguageController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            if (JsonLocalizer.SupportedCultures.Contains(culture))
            {
                Response.Cookies.Append(JsonLocalizer.CookieName, culture, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,
                });
            }

            return LocalRedirect(Url.IsLocalUrl(returnUrl) ? returnUrl : "/");
        }
    }
}
