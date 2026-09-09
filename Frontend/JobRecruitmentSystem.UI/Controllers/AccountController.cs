using System.Security.Claims;
using JobRecruitmentSystem.UI.Models.Auth;
using JobRecruitmentSystem.UI.Models.Employer;
using JobRecruitmentSystem.UI.Models.JobSeeker;
using JobRecruitmentSystem.UI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace JobRecruitmentSystem.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiClient _api;

        public AccountController(ApiClient api)
        {
            _api = api;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _api.PostAsync<AuthResponse>("Auth/register", model);
                TempData["Success"] = "auth.registerSuccess";
                return RedirectToAction(nameof(ConfirmEmail), new { email = model.Email });
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ConfirmEmail(string? email)
        {
            return View(new ConfirmEmailViewModel { Email = email ?? string.Empty });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _api.PostAsync("Auth/confirm-email", model);
                TempData["Success"] = "auth.confirmSuccess";
                return RedirectToAction(nameof(Login));
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _api.PostAsync<AuthResponse>("Auth/login", model);
                if (result is null || string.IsNullOrEmpty(result.Token))
                {
                    ModelState.AddModelError(string.Empty, "errors.generic");
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, result.UserId.ToString()),
                    new(ClaimTypes.Name, result.FullName),
                    new(ClaimTypes.Email, result.Email),
                    new(ClaimTypes.Role, result.Role),
                    new(ClaimsPrincipalExtensions.AccessTokenClaimType, result.Token),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                if (result.Role == "Employer")
                {
                    var employerProfile = await TryGetAsync<EmployerProfileViewModel>("Employer/profile", result.Token);
                    if (employerProfile is null || string.IsNullOrWhiteSpace(employerProfile.CompanyName))
                    {
                        TempData["Info"] = "employer.completeProfilePrompt";
                        return RedirectToAction("Profile", "Employer");
                    }
                }
                else if (result.Role == "JobSeeker")
                {
                    var seekerProfile = await TryGetAsync<JobSeekerProfileViewModel>("JobSeeker/profile", result.Token);
                    var isEmpty = seekerProfile is null
                        || (string.IsNullOrWhiteSpace(seekerProfile.Skills)
                            && string.IsNullOrWhiteSpace(seekerProfile.WorkExperience)
                            && string.IsNullOrWhiteSpace(seekerProfile.CvFilePath));

                    if (isEmpty)
                    {
                        TempData["Info"] = "jobseeker.completeProfilePrompt";
                        return RedirectToAction("Profile", "JobSeeker");
                    }
                }

                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                return result.Role switch
                {
                    "Admin" => RedirectToAction("Users", "Admin"),
                    _ => RedirectToAction("Index", "Home"),
                };
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _api.PostAsync("Auth/forgot-password", model);
                TempData["Success"] = "auth.forgotSuccess";
                return RedirectToAction(nameof(ResetPassword), new { email = model.Email });
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ResetPassword(string? email)
        {
            return View(new ResetPasswordViewModel { Email = email ?? string.Empty });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _api.PostAsync("Auth/reset-password", model);
                TempData["Success"] = "auth.resetSuccess";
                return RedirectToAction(nameof(Login));
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task<T?> TryGetAsync<T>(string url, string? overrideToken = null) where T : class
        {
            try
            {
                return await _api.GetAsync<T>(url, overrideToken);
            }
            catch (ApiException)
            {
                return null;
            }
        }
    }
}
