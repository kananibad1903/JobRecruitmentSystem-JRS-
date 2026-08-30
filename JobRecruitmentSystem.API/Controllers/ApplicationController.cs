namespace JobRecruitmentSystem.API.Controllers
{
    using global::JobRecruitmentSystem.BLL.DTOs;
    using global::JobRecruitmentSystem.BLL.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    namespace JobRecruitmentSystem.API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
        public class ApplicationController : ControllerBase
        {
            private readonly IApplicationService _applicationService;

            public ApplicationController(IApplicationService applicationService)
            {
                _applicationService = applicationService;
            }

            [HttpPost]
            public async Task<IActionResult> Apply([FromBody] CreateApplicationDto dto)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                try
                {
                    var application = await _applicationService.ApplyAsync(userId, dto);
                    return Ok(application);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            [HttpGet("my-applications")]
            public async Task<IActionResult> GetMyApplications()
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                try
                {
                    var applications = await _applicationService.GetMyApplicationsAsync(userId);
                    return Ok(applications);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            [HttpGet("job-post/{jobPostId}")]
            public async Task<IActionResult> GetApplicationsForJobPost(int jobPostId)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                try
                {
                    var applications = await _applicationService.GetApplicationsForJobPostAsync(userId, jobPostId);
                    return Ok(applications);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            [HttpPut("{id}/status")]
            public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateApplicationStatusDto dto)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                try
                {
                    var application = await _applicationService.UpdateStatusAsync(userId, id, dto);
                    return Ok(application);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
        }
    }
}
