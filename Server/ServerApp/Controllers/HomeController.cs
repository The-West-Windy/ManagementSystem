using Microsoft.AspNetCore.Mvc;

namespace ServerApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
