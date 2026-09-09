using JobRecruitmentSystem.UI.Models.Employer;
using JobRecruitmentSystem.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace JobRecruitmentSystem.UI.Filters
{
    /// <summary>
    /// Blocks an Employer with an unfilled company profile (CompanyName still
    /// empty, as set by registration) from reaching any page other than
    /// their own profile form, Account actions, or the language switcher.
    /// </summary>
    public class RequireCompanyProfileFilter : IAsyncActionFilter
    {
        private readonly ApiClient _api;
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public RequireCompanyProfileFilter(ApiClient api, ITempDataDictionaryFactory tempDataFactory)
        {
            _api = api;
            _tempDataFactory = tempDataFactory;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;

            if (user.Identity is { IsAuthenticated: true } && user.IsInRole("Employer"))
            {
                var controllerName = context.RouteData.Values["controller"]?.ToString();
                var actionName = context.RouteData.Values["action"]?.ToString();

                var isExempt = controllerName == "Account"
                    || controllerName == "Language"
                    || (controllerName == "Employer" && actionName == "Profile");

                if (!isExempt)
                {
                    try
                    {
                        var profile = await _api.GetAsync<EmployerProfileViewModel>("Employer/profile");
                        if (profile is null || string.IsNullOrWhiteSpace(profile.CompanyName))
                        {
                            var tempData = _tempDataFactory.GetTempData(context.HttpContext);
                            tempData["Info"] = "employer.completeProfilePrompt";
                            context.Result = new RedirectToActionResult("Profile", "Employer", null);
                        }
                    }
                    catch (ApiException)
                    {
                        // API unreachable / profile lookup failed — don't lock the user out over it.
                    }
                }
            }

            if (context.Result is null)
            {
                await next();
            }
        }
    }
}
