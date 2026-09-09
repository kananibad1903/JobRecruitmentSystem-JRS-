using JobRecruitmentSystem.UI.Models.Notification;
using JobRecruitmentSystem.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobRecruitmentSystem.UI.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly ApiClient _api;

        public NotificationController(ApiClient api)
        {
            _api = api;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            try
            {
                var notifications = await _api.GetAsync<List<NotificationViewModel>>("Notification");
                return Json(notifications ?? new List<NotificationViewModel>());
            }
            catch (ApiException)
            {
                return Json(new List<NotificationViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> UnreadCount()
        {
            try
            {
                var result = await _api.GetAsync<UnreadCountResponse>("Notification/unread-count");
                return Json(new { count = result?.Count ?? 0 });
            }
            catch (ApiException)
            {
                return Json(new { count = 0 });
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarkRead(int id)
        {
            try
            {
                await _api.PutAsync($"Notification/{id}/read", null);
            }
            catch (ApiException)
            {
                // best-effort — nothing useful to surface to the caller here
            }

            return Json(new { ok = true });
        }

        [HttpPost]
        public async Task<IActionResult> MarkAllRead()
        {
            try
            {
                await _api.PutAsync("Notification/read-all", null);
            }
            catch (ApiException)
            {
                // best-effort
            }

            return Json(new { ok = true });
        }

        private class UnreadCountResponse
        {
            public int Count { get; set; }
        }
    }
}
