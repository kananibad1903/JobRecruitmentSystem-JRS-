namespace JobRecruitmentSystem.API.Controllers
{
    using global::JobRecruitmentSystem.BLL.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;

    namespace JobRecruitmentSystem.API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
        public class SavedJobController : ControllerBase
        {
            private readonly ISavedJobService _savedJobService;

            public SavedJobController(ISavedJobService savedJobService)
            {
                _savedJobService = savedJobService;
            }

            [HttpPost("{jobPostId}")]
            public async Task<IActionResult> Save(int jobPostId)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                try
                {
                    var savedJob = await _savedJobService.SaveAsync(userId, jobPostId);
                    return Ok(savedJob);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            [HttpGet]
            public async Task<IActionResult> GetMySavedJobs()
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                try
                {
                    var savedJobs = await _savedJobService.GetMySavedJobsAsync(userId);
                    return Ok(savedJobs);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }

            [HttpDelete("{jobPostId}")]
            public async Task<IActionResult> Remove(int jobPostId)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                try
                {
                    await _savedJobService.RemoveAsync(userId, jobPostId);
                    return Ok(new { message = "Elan siyahıdan silindi." });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = ex.Message });
                }
            }
        }
    }
}
