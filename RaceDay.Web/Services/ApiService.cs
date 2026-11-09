using RaceDay.Web.Models;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using RaceDay.Web.ViewModels;
using System.Net.Http.Json;

namespace RaceDay.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        // CATEGORIES

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Get, "API/Categories");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var categories = await response.Content
                .ReadFromJsonAsync<List<Category>>();

            return categories ?? new List<Category>();
        }

        public async Task<bool> CreateCategoryAsync(Category model)
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Post, "API/Categories");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(new
            {
                categoryName = model.CategoryName,
                description = model.Description
            });

            using var response = await _httpClient.SendAsync(request);

            var errorMessage = await response.Content.ReadAsStringAsync();

            Console.WriteLine("CATEGORY STATUS: " + response.StatusCode);
            Console.WriteLine("CATEGORY RESPONSE: " + errorMessage);

            return response.IsSuccessStatusCode;
        }

        // EVENTS

        public async Task<List<Event>> GetEventsAsync()
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Get, "API/Events");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var events = await response.Content
                .ReadFromJsonAsync<List<Event>>();

            return events ?? new List<Event>();
        }

        public async Task<bool> CreateEventAsync(Event model)
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Post, "API/Events");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(new
            {
                eventName = model.EventName,
                eventDate = model.EventDate,
                categoryID = model.CategoryID,
                locationID = model.LocationID
            });

            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEventAsync(int id, Event model)
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Put, $"API/Events/{id}");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(new
            {
                eventName = model.EventName,
                eventDate = model.EventDate,
                categoryID = model.CategoryID,
                locationID = model.LocationID
            });

            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Delete, $"API/Events/{id}");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        // EVENT ENROLLMENT

        public async Task<List<Enrollment>> GetEnrollmentsAsync()
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Get, "API/EventEnrollment");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var enrollments = await response.Content
                .ReadFromJsonAsync<List<Enrollment>>();

            return enrollments ?? new List<Enrollment>();
        }

        public async Task<bool> CreateEnrollmentAsync(Enrollment model)
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Post, "API/EventEnrollment");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(new
            {
                userID = model.UserID,
                eventID = model.EventID
            });

            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEnrollmentAsync(int id)
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Delete, $"API/EventEnrollment/{id}");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        // LOCATION

        public async Task<List<Location>> GetLocationsAsync()
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Get, "API/Location");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var locations = await response.Content
                .ReadFromJsonAsync<List<Location>>();

            return locations ?? new List<Location>();
        }

        public async Task<bool> CreateLocationAsync(Location model)
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Post, "API/Location");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(new
            {
                locationName = model.LocationName,
                address = model.Address
            });

            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateLocationAsync(int id, Location model)
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Put, $"API/Location/{id}");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(new
            {
                locationName = model.LocationName,
                address = model.Address
            });

            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteLocationAsync(int id)
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Delete, $"API/Location/{id}");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        // RESULTS

        public async Task<List<Result>> GetResultsAsync()
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Get, "API/Results");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var results = await response.Content
                .ReadFromJsonAsync<List<Result>>();

            return results ?? new List<Result>();
        }

        public async Task<bool> CreateResultAsync(Result model)
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Post, "API/Results");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(new
            {
                enrollmentID = model.EnrollmentID,
                position = model.Position
            });

            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateResultAsync(int id, Result model)
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Put, $"API/Results/{id}");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(new
            {
                enrollmentID = model.EnrollmentID,
                position = model.Position
            });

            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteResultAsync(int id)
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Delete, $"API/Results/{id}");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateUserAsync(int id, ProfileViewModel model)
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            if (string.IsNullOrWhiteSpace(token))
                throw new UnauthorizedAccessException("Please log in first.");

            using var request = new HttpRequestMessage(
                HttpMethod.Put, $"API/Users/{id}");

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Content = JsonContent.Create(new
            {
                firstName = model.FirstName,
                lastName = model.LastName,
                email = model.Email
            });

            using var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        // USER REGISTRATION

        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "API/Users",
                model
            );

            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine("REGISTER STATUS: " + response.StatusCode);
            Console.WriteLine("REGISTER RESPONSE: " + responseBody);

            return response.IsSuccessStatusCode;
        }

        // LOGIN

        public async Task<string?> LoginAsync(LoginViewModel login)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "API/Users/Login", login);

            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine("LOGIN STATUS: " + response.StatusCode);
            Console.WriteLine("LOGIN RESPONSE: " + responseBody);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = System.Text.Json.JsonSerializer.Deserialize<LoginResponse>(
                responseBody,
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (result == null || string.IsNullOrWhiteSpace(result.Token))
                return null;

            var session = _httpContextAccessor.HttpContext?.Session;

            session?.SetString("JWToken", result.Token);
            session?.SetString("UserID", result.UserID.ToString());
            session?.SetString("FirstName", result.FirstName ?? "");
            session?.SetString("LastName", result.LastName ?? "");
            session?.SetString("Email", result.Email ?? "");
            session?.SetString("Role", result.Role ?? "");

            return result.Token;
        }

        private class LoginResponse
        {
            public string? Token { get; set; }
            public int UserID { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Email { get; set; }
            public string? Role { get; set; }
        }
    }
}