using Microsoft.AspNetCore.Mvc;

namespace JobRecruitmentSystem.API.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
