using Microsoft.AspNetCore.Mvc;
using JobRecruitmentSystem.BLL.DTOs;
using JobRecruitmentSystem.BLL.Services.Interfaces;

namespace JobRecruitmentSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiChatController : ControllerBase
    {
        private readonly IAiChatService _aiChatService;

        public AiChatController(IAiChatService aiChatService)
        {
            _aiChatService = aiChatService;
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatMessageDto dto)
        {
            try
            {
                var reply = await _aiChatService.SendMessageAsync(dto.Message);
                return Ok(new ChatResponseDto { Reply = reply });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}