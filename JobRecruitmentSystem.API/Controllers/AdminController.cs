using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JobRecruitmentSystem.BLL.Services.Interfaces;

namespace JobRecruitmentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _adminService.DeleteUserAsync(id);
            return Ok(new { message = "İstifadəçi silindi." });
        }

        [HttpGet("jobposts")]
        public async Task<IActionResult> GetAllJobPosts()
        {
            var jobPosts = await _adminService.GetAllJobPostsAsync();
            return Ok(jobPosts);
        }

        [HttpGet("jobposts/pending")]
        public async Task<IActionResult> GetPendingJobPosts()
        {
            var jobPosts = await _adminService.GetPendingJobPostsAsync();
            return Ok(jobPosts);
        }

        [HttpPut("jobposts/{id}/approve")]
        public async Task<IActionResult> ApproveJobPost(int id)
        {
            await _adminService.ApproveJobPostAsync(id);
            return Ok(new { message = "Elan təsdiqləndi." });
        }

        [HttpDelete("jobposts/{id}")]
        public async Task<IActionResult> DeleteJobPost(int id)
        {
            await _adminService.DeleteJobPostAsync(id);
            return Ok(new { message = "Elan silindi." });
        }
    }
}