
using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly ApiService _apiService;

        public ParticipantController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            var userID = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userID))
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.FirstName = HttpContext.Session.GetString("FirstName");
            ViewBag.LastName = HttpContext.Session.GetString("LastName");

            return View();
        }

        public async Task<IActionResult> Events()
        {
            var userID = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userID))
            {
                return RedirectToAction("Index", "Login");
            }

            var events = await _apiService.GetEventsAsync();

            return View(events);
        }

        public async Task<IActionResult> Enrollments()
        {
            var userID = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userID))
            {
                return RedirectToAction("Index", "Login");
            }

            var enrollments = await _apiService.GetEnrollmentsAsync();

            var myEnrollments = enrollments
                .Where(e => e.UserID == int.Parse(userID))
                .ToList();

            return View(myEnrollments);
        }

        public async Task<IActionResult> Results()
        {
            var userID = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userID))
            {
                return RedirectToAction("Index", "Login");
            }

            var results = await _apiService.GetResultsAsync();

            return View(results);
        }
    }
}
