
using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class EventEnrollmentController : Controller
    {
        private readonly ApiService _apiService;

        public EventEnrollmentController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var enrollments = await _apiService.GetEnrollmentsAsync();
                return View(enrollments);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Enrollment model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                bool success = await _apiService.CreateEnrollmentAsync(model);

                if (success)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Unable to create enrollment.");
                return View(model);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool success = await _apiService.DeleteEnrollmentAsync(id);

                if (!success)
                    TempData["Error"] = "Unable to delete enrollment.";

                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
