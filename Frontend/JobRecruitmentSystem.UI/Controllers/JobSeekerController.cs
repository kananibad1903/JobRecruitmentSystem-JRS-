using JobRecruitmentSystem.UI.Models.Application;
using JobRecruitmentSystem.UI.Models.JobSeeker;
using JobRecruitmentSystem.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobRecruitmentSystem.UI.Controllers
{
    [Authorize(Roles = "JobSeeker")]
    public class JobSeekerController : Controller
    {
        private readonly ApiClient _api;

        public JobSeekerController(ApiClient api)
        {
            _api = api;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            try
            {
                var profile = await _api.GetAsync<JobSeekerProfileViewModel>("JobSeeker/profile");
                return View(profile ?? new JobSeekerProfileViewModel());
            }
            catch (ApiException ex)
            {
                TempData["Error"] = ex.Message;
                return View(new JobSeekerProfileViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(JobSeekerProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _api.PutAsync("JobSeeker/profile", new { model.Skills, model.WorkExperience });
                TempData["Success"] = "jobseeker.profileUpdated";
            }
            catch (ApiException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }

            return RedirectToAction(nameof(Profile));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadCv(IFormFile? file)
        {
            if (file is null || file.Length == 0)
            {
                TempData["Error"] = "errors.generic";
                return RedirectToAction(nameof(Profile));
            }

            try
            {
                using var content = new MultipartFormDataContent();
                await using var stream = file.OpenReadStream();
                using var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                content.Add(streamContent, "file", file.FileName);

                await _api.PostMultipartAsync<JobSeekerProfileViewModel>("JobSeeker/upload-cv", content);
                TempData["Success"] = "jobseeker.cvUploaded";
            }
            catch (ApiException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        public async Task<IActionResult> MyApplications()
        {
            var applications = await _api.GetAsync<List<ApplicationViewModel>>("Application/my-applications");
            return View(applications ?? new List<ApplicationViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> SavedJobs()
        {
            var savedJobs = await _api.GetAsync<List<SavedJobViewModel>>("SavedJob");
            return View(savedJobs ?? new List<SavedJobViewModel>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSavedJob(int jobPostId)
        {
            try
            {
                await _api.DeleteAsync($"SavedJob/{jobPostId}");
            }
            catch (ApiException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(SavedJobs));
        }
    }
}
