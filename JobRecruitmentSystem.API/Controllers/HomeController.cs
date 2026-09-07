using Microsoft.AspNetCore.Mvc;

namespace JobRecruitmentSystem.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
