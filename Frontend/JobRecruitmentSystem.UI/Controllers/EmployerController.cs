using JobRecruitmentSystem.UI.Models.Application;
using JobRecruitmentSystem.UI.Models.Employer;
using JobRecruitmentSystem.UI.Models.JobPost;
using JobRecruitmentSystem.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobRecruitmentSystem.UI.Controllers
{
    [Authorize(Roles = "Employer")]
    public class EmployerController : Controller
    {
        private readonly ApiClient _api;

        public EmployerController(ApiClient api)
        {
            _api = api;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            try
            {
                var profile = await _api.GetAsync<EmployerProfileViewModel>("Employer/profile");
                return View(profile ?? new EmployerProfileViewModel());
            }
            catch (ApiException ex)
            {
                TempData["Error"] = ex.Message;
                return View(new EmployerProfileViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(EmployerProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _api.PutAsync("Employer/profile", new
                {
                    model.CompanyName,
                    model.CompanyDescription,
                    model.CompanyLocation,
                    model.Website,
                });
                TempData["Success"] = "employer.profileUpdated";
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }

            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        public async Task<IActionResult> MyPosts()
        {
            var posts = await _api.GetAsync<List<JobPostViewModel>>("JobPost/my-posts");
            return View(posts ?? new List<JobPostViewModel>());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new JobPostFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobPostFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _api.PostAsync<JobPostViewModel>("JobPost", model);
                TempData["Success"] = "employer.postCreated";
                return RedirectToAction(nameof(MyPosts));
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var job = await _api.GetAsync<JobPostViewModel>($"JobPost/{id}");
                if (job is null)
                {
                    return NotFound();
                }

                var model = new JobPostFormViewModel
                {
                    Id = job.Id,
                    Title = job.Title,
                    Description = job.Description,
                    Requirements = job.Requirements,
                    Category = job.Category,
                    Location = job.Location,
                    JobType = job.JobType,
                    SalaryMin = job.SalaryMin,
                    SalaryMax = job.SalaryMax,
                    Deadline = job.Deadline,
                };

                return View(model);
            }
            catch (ApiException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JobPostFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _api.PutAsync($"JobPost/{id}", model);
                TempData["Success"] = "employer.postUpdated";
                return RedirectToAction(nameof(MyPosts));
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _api.DeleteAsync($"JobPost/{id}");
                TempData["Success"] = "employer.postDeleted";
            }
            catch (ApiException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(MyPosts));
        }

        [HttpGet]
        public async Task<IActionResult> CandidateProfile(int id)
        {
            try
            {
                var profile = await _api.GetAsync<CandidateProfileViewModel>($"Employer/candidates/{id}");
                if (profile is null)
                {
                    return NotFound();
                }

                return View(profile);
            }
            catch (ApiException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(MyPosts));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Applications(int jobPostId)
        {
            var applications = await _api.GetAsync<List<ApplicationViewModel>>($"Application/job-post/{jobPostId}");
            ViewBag.JobPostId = jobPostId;
            return View(applications ?? new List<ApplicationViewModel>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(UpdateApplicationStatusViewModel model)
        {
            try
            {
                await _api.PutAsync($"Application/{model.Id}/status", new { model.Status, model.EmployerNote });
            }
            catch (ApiException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Applications), new { jobPostId = model.JobPostId });
        }
    }
}
