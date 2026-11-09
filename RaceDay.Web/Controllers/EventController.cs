using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApiService _apiService;

        public EventsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        private async Task LoadDropdowns()
        {
            ViewBag.Categories = await _apiService.GetCategoriesAsync();
            ViewBag.Locations = await _apiService.GetLocationsAsync();
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var events = await _apiService.GetEventsAsync();
                return View(events);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                await LoadDropdowns();
                return View();
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadDropdowns();
                    return View(model);
                }

                bool success = await _apiService.CreateEventAsync(model);

                if (success)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Unable to create event.");

                await LoadDropdowns();
                return View(model);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var events = await _apiService.GetEventsAsync();

                var eventItem = events.FirstOrDefault(e => e.EventID == id);

                if (eventItem == null)
                    return NotFound();

                await LoadDropdowns();

                return View(eventItem);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Event model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadDropdowns();
                    return View(model);
                }

                bool success = await _apiService.UpdateEventAsync(id, model);

                if (success)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Unable to update event.");

                await LoadDropdowns();
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
                bool success = await _apiService.DeleteEventAsync(id);

                if (!success)
                    TempData["Error"] = "Unable to delete event.";

                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}