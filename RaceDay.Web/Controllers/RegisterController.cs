using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Services;
using RaceDay.Web.ViewModels;

namespace RaceDay.Web.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApiService _apiService;

        public RegisterController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var registered = await _apiService.RegisterAsync(model);

            if (!registered)
            {
                ViewBag.Error = "Registration failed. Please check your details or try another email.";
                return View(model);
            }

            TempData["Success"] = "Your account has been created successfully. Please log in.";

            return RedirectToAction("Index", "Login");
        }
    }
}