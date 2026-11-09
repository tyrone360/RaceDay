
using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class ResultsController : Controller
    {
        private readonly ApiService _apiService;

        public ResultsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // DISPLAY RESULTS
        public async Task<IActionResult> Index()
        {
            try
            {
                var results = await _apiService.GetResultsAsync();
                return View(results);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // CREATE RESULT
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Result model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                bool success = await _apiService.CreateResultAsync(model);

                if (success)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Unable to create result.");
                return View(model);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // EDIT RESULT
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var results = await _apiService.GetResultsAsync();

                var result = results.FirstOrDefault(r => r.ResultID == id);

                if (result == null)
                    return NotFound();

                return View(result);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Result model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                bool success = await _apiService.UpdateResultAsync(id, model);

                if (success)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Unable to update result.");
                return View(model);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }

        // DELETE RESULT
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool success = await _apiService.DeleteResultAsync(id);

                if (!success)
                    TempData["Error"] = "Unable to delete result.";

                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}
