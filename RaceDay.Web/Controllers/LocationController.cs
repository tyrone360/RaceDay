
using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class LocationController : Controller
    {
        private readonly ApiService _apiService;

        public LocationController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // DISPLAY LOCATIONS
        public async Task<IActionResult> Index()
        {
            try
            {
                var locations = await _apiService.GetLocationsAsync();
                return View(locations);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // CREATE LOCATION
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Location model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                bool success = await _apiService.CreateLocationAsync(model);

                if (success)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Unable to create location.");
                return View(model);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // EDIT LOCATION
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var locations = await _apiService.GetLocationsAsync();

                var location = locations.FirstOrDefault(
                    l => l.LocationID == id);

                if (location == null)
                    return NotFound();

                return View(location);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Location model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                bool success = await _apiService.UpdateLocationAsync(id, model);

                if (success)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Unable to update location.");
                return View(model);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // DELETE LOCATION
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool success = await _apiService.DeleteLocationAsync(id);

                if (!success)
                    TempData["Error"] = "Unable to delete location.";

                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
