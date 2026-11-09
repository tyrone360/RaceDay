
using Microsoft.AspNetCore.Mvc;
using project.Backend;
using User_Register.Controllers;

namespace project.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class user_asking_controller : ControllerBase
    {
        // POST: API/user_asking_controller/User_Info
        [HttpPost("User_Info")]
        public IActionResult User_Info([FromBody] UserRequest user)
        {
            if (user == null)
            {
                return BadRequest("User information is required.");
            }

            if (string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Password is required.");
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                return BadRequest("Email is required.");
            }

            var logger = HttpContext.RequestServices
     .GetRequiredService<ILogger<dataCollected>>();

            dataCollected store_data = new dataCollected(logger);

            string message = store_data.personal_Information(
                user.FirstName,
                user.LastName,
                user.Age,
                user.Email,
                user.Password
            );

            if (message == "User account created successfully")
            {
                return Ok(message);
            }

            return BadRequest(message);
        }
    }
}
