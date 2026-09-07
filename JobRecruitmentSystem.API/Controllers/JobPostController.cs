using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobRecruitmentSystem.BLL.DTOs;
using JobRecruitmentSystem.BLL.Services.Interfaces;

namespace JobRecruitmentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobPostController : ControllerBase
    {
        private readonly IJobPostService _jobPostService;

        public JobPostController(IJobPostService jobPostService)
        {
            _jobPostService = jobPostService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var jobPosts = await _jobPostService.GetAllAsync();
            return Ok(jobPosts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var jobPost = await _jobPostService.GetByIdAsync(id);
                return Ok(jobPost);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string category,
            [FromQuery] string location,
            [FromQuery] string jobType,
            [FromQuery] decimal? minSalary,
            [FromQuery] decimal? maxSalary)
        {
            var jobPosts = await _jobPostService.SearchAsync(category, location, jobType, minSalary, maxSalary);
            return Ok(jobPosts);
        }

        [HttpGet("my-posts")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetMyPosts()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var jobPosts = await _jobPostService.GetByEmployerUserIdAsync(userId);
                return Ok(jobPosts);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Create([FromBody] CreateJobPostDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var jobPost = await _jobPostService.CreateAsync(userId, dto);
                return Ok(jobPost);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateJobPostDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                var jobPost = await _jobPostService.UpdateAsync(userId, id, dto);
                return Ok(jobPost);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                await _jobPostService.DeleteAsync(userId, id);
                return Ok(new { message = "Elan silindi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}