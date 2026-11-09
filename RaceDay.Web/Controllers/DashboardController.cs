using Microsoft.AspNetCore.Mvc;

namespace RaceDay.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}