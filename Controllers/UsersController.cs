using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace User_Register.Controllers
{

    public class UserRequest
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;

        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; } = string.Empty;
    }



    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }



    [Route("API/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string connection =
            @"Data Source=(localdb)\project;Database=project;";


        [HttpGet]
        public IActionResult GetUsers()
        {
            List<object> users = new List<object>();

            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = @"
    SELECT UserID, FirstName, LastName, Email
    FROM dbo.Users";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    connect.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new Dictionary<string, object?>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                user[reader.GetName(i)] =
                                    reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }

                            users.Add(user);
                        }
                    }
                }
            }

            return Ok(users);
        }



        [HttpPost]
        public IActionResult AddUser([FromBody] UserRequest user)
        {
            if (user == null)
            {
                return BadRequest("User information is required.");
            }

            if (string.IsNullOrWhiteSpace(user.Email))
            {
                return BadRequest("Email is required.");
            }

            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = @"
            INSERT INTO dbo.Users (FirstName, LastName, Email)
            VALUES (@FirstName, @LastName, @Email)";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Email", user.Email);

                    try
                    {
                        connect.Open();

                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            return Ok("User added successfully.");
                        }

                        return BadRequest("User was not added.");
                    }
                    catch (SqlException ex)
                        when (ex.Number == 2627 || ex.Number == 2601)
                    {
                        return Conflict(
                            "An account with this email already exists.");
                    }
                    catch (SqlException)
                    {
                        return StatusCode(
                            500,
                            "Unable to add user right now. Please try again later.");
                    }
                }
            }
        }




        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "DELETE FROM Users WHERE UserID = @UserID";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@UserID", id);

                    try
                    {
                        connect.Open();

                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            return Ok("User deleted successfully.");
                        }

                        return NotFound("User not found.");
                    }
                    catch (SqlException ex) when (ex.Number == 547)
                    {
                        return Conflict(
                            "This user cannot be deleted because they are linked to an event enrollment."
                        );
                    }
                }
            }
        }

        // data that will put the PUT method
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserRequest user)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                return BadRequest("Email is required.");
            }

            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = @"
                    UPDATE Users
                    SET FirstName = @FirstName,
                        LastName = @LastName,
                        Email = @Email
                    WHERE UserID = @UserID";


                //string connection that will
                //allow the useer details
                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@UserID", id);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Email", user.Email);

                    connect.Open();
                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        return Ok("User updated successfully.");
                    }

                    return NotFound("User not found.");
                }
            }
        }


        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = @"
            SELECT UserID, FirstName, LastName, Email, Password
            FROM dbo.Users
            WHERE Email = @Email";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 255)
                        .Value = request.Email.Trim();

                    connect.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            return Unauthorized("Invalid email or password.");
                        }

                        int userId = reader.GetInt32(0);
                        string firstName = reader.GetString(1);
                        string lastName = reader.GetString(2);
                        string email = reader.GetString(3);

                        string? storedHash = reader.IsDBNull(4)
                            ? null
                            : reader.GetString(4);

                        if (string.IsNullOrWhiteSpace(storedHash))
                        {
                            return Unauthorized("Invalid email or password.");
                        }

                        var hasher =
                            new Microsoft.AspNetCore.Identity.PasswordHasher<object>();

                        var result = hasher.VerifyHashedPassword(
                            new object(),
                            storedHash,
                            request.Password);

                        if (result ==
                            Microsoft.AspNetCore.Identity.PasswordVerificationResult.Failed)
                        {
                            return Unauthorized("Invalid email or password.");
                        }

                        return Ok(new
                        {
                            Message = "Login successful.",
                            UserID = userId,
                            FirstName = firstName,
                            LastName = lastName,
                            Email = email
                        });
                    }
                }
            }
        }


    }
}