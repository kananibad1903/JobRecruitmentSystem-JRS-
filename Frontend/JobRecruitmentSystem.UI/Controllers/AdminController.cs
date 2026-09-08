using JobRecruitmentSystem.UI.Models.Admin;
using JobRecruitmentSystem.UI.Models.JobPost;
using JobRecruitmentSystem.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobRecruitmentSystem.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApiClient _api;

        public AdminController(ApiClient api)
        {
            _api = api;
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = await _api.GetAsync<List<AdminUserViewModel>>("Admin/users");
            return View(users ?? new List<AdminUserViewModel>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _api.DeleteAsync($"Admin/users/{id}");
            }
            catch (ApiException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        public async Task<IActionResult> JobPosts()
        {
            var jobPosts = await _api.GetAsync<List<JobPostViewModel>>("Admin/jobposts");
            return View(jobPosts ?? new List<JobPostViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> PendingJobPosts()
        {
            var jobPosts = await _api.GetAsync<List<JobPostViewModel>>("Admin/jobposts/pending");
            return View(jobPosts ?? new List<JobPostViewModel>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, string returnAction = nameof(PendingJobPosts))
        {
            try
            {
                await _api.PutAsync($"Admin/jobposts/{id}/approve", null);
            }
            catch (ApiException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(returnAction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteJobPost(int id, string returnAction = nameof(JobPosts))
        {
            try
            {
                await _api.DeleteAsync($"Admin/jobposts/{id}");
            }
            catch (ApiException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(returnAction);
        }
    }
}
