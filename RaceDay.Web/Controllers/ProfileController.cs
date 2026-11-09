using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Services;
using RaceDay.Web.ViewModels;

namespace RaceDay.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApiService _apiService;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(
            ApiService apiService,
            IWebHostEnvironment environment)
        {
            _apiService = apiService;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userID = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userID))
                return RedirectToAction("Index", "Login");

            var model = new ProfileViewModel
            {
                FirstName = HttpContext.Session.GetString("FirstName") ?? "",
                LastName = HttpContext.Session.GetString("LastName") ?? "",
                Email = HttpContext.Session.GetString("Email") ?? "",
                ProfilePicture = HttpContext.Session.GetString("ProfilePicture")
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProfileViewModel model)
        {
            var userID = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(userID))
                return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
                return View(model);

            var updated = await _apiService.UpdateUserAsync(
                int.Parse(userID), model);

            if (!updated)
            {
                ViewBag.Error = "Unable to update your profile.";
                return View(model);
            }

            HttpContext.Session.SetString("FirstName", model.FirstName);
            HttpContext.Session.SetString("LastName", model.LastName);
            HttpContext.Session.SetString("Email", model.Email);

            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
                var extension = Path.GetExtension(model.ProfileImage.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                {
                    ViewBag.Error = "Please upload a JPG, PNG or WEBP image.";
                    return View(model);
                }

                if (model.ProfileImage.Length > 5 * 1024 * 1024)
                {
                    ViewBag.Error = "The image must be smaller than 5 MB.";
                    return View(model);
                }

                var uploadFolder = Path.Combine(
                    _environment.WebRootPath, "uploads", "profiles");

                Directory.CreateDirectory(uploadFolder);

                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }

                var imagePath = "/uploads/profiles/" + fileName;

                HttpContext.Session.SetString("ProfilePicture", imagePath);
                model.ProfilePicture = imagePath;
            }

            TempData["Success"] = "Your profile has been updated successfully.";

            return RedirectToAction("Index");
        }
    }
}