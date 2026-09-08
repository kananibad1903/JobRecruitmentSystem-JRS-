using JobRecruitmentSystem.UI.Models.JobPost;
using JobRecruitmentSystem.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobRecruitmentSystem.UI.Controllers
{
    public class JobPostController : Controller
    {
        private readonly ApiClient _api;

        public JobPostController(ApiClient api)
        {
            _api = api;
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var job = await _api.GetAsync<JobPostViewModel>($"JobPost/{id}");
                if (job is null)
                {
                    return NotFound();
                }

                var model = new JobPostDetailViewModel
                {
                    Job = job,
                    CanApply = User.IsInRole("JobSeeker"),
                    CanSave = User.IsInRole("JobSeeker"),
                };

                return View(model);
            }
            catch (ApiException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "JobSeeker")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(int id)
        {
            try
            {
                await _api.PostAsync("Application", new { jobPostId = id });
                TempData["Success"] = "job.applySuccess";
            }
            catch (ApiException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [Authorize(Roles = "JobSeeker")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(int id)
        {
            try
            {
                await _api.PostAsync($"SavedJob/{id}");
                TempData["Success"] = "job.saveSuccess";
            }
            catch (ApiException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
