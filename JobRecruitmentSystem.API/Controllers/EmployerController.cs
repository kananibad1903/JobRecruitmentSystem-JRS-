using Microsoft.AspNetCore.Mvc;
using global::JobRecruitmentSystem.BLL.DTOs;
using global::JobRecruitmentSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace JobRecruitmentSystem.API.Controllers
{
    namespace JobRecruitmentSystem.API.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        [Authorize]
        public class EmployerController : ControllerBase
        {
            private readonly IEmployerService _employerService;

            public EmployerController(IEmployerService employerService)
            {
                _employerService = employerService;
            }

            [HttpGet("profile")]
            public async Task<IActionResult> GetProfile()
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                try
                {
                    var profile = await _employerService.GetByUserIdAsync(userId);
                    return Ok(profile);
                }
                catch (Exception ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }

            [HttpPut("profile")]
            public async Task<IActionResult> UpdateProfile([FromBody] UpdateEmployerProfileDto dto)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                try
                {
                    var updated = await _employerService.UpdateProfileAsync(userId, dto);
                    return Ok(updated);
                }
                catch (Exception ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }
        }
    }
}
