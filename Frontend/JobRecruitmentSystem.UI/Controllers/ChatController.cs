using JobRecruitmentSystem.UI.Models.Chat;
using JobRecruitmentSystem.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobRecruitmentSystem.UI.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApiClient _api;

        public ChatController(ApiClient api)
        {
            _api = api;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new { message = "errors.generic" });
            }

            try
            {
                var response = await _api.PostAsync<ChatResponse>("AiChat", request);
                return Json(new { reply = response?.Reply ?? string.Empty });
            }
            catch (ApiException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
