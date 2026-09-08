using System.Text;
using JobRecruitmentSystem.UI.Models.JobPost;
using JobRecruitmentSystem.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobRecruitmentSystem.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiClient _api;

        public HomeController(ApiClient api)
        {
            _api = api;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? category, string? location, string? jobType, decimal? minSalary, decimal? maxSalary)
        {
            var hasFilter = !string.IsNullOrWhiteSpace(category)
                || !string.IsNullOrWhiteSpace(location)
                || !string.IsNullOrWhiteSpace(jobType)
                || minSalary.HasValue
                || maxSalary.HasValue;

            List<JobPostViewModel>? jobs;

            if (hasFilter)
            {
                var query = new StringBuilder("JobPost/search?");
                query.Append($"category={Uri.EscapeDataString(category ?? string.Empty)}&");
                query.Append($"location={Uri.EscapeDataString(location ?? string.Empty)}&");
                query.Append($"jobType={Uri.EscapeDataString(jobType ?? string.Empty)}");
                if (minSalary.HasValue) query.Append($"&minSalary={minSalary.Value}");
                if (maxSalary.HasValue) query.Append($"&maxSalary={maxSalary.Value}");

                jobs = await _api.GetAsync<List<JobPostViewModel>>(query.ToString());
            }
            else
            {
                jobs = await _api.GetAsync<List<JobPostViewModel>>("JobPost");
            }

            var model = new JobPostListViewModel
            {
                Jobs = jobs ?? new List<JobPostViewModel>(),
                Category = category,
                Location = location,
                JobType = jobType,
                MinSalary = minSalary,
                MaxSalary = maxSalary,
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View(new Models.Shared.ErrorViewModel
            {
                RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            });
        }
    }
}
