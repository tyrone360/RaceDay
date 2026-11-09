using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace project.Backend
{
    public class dataCollected
    {
        private readonly ILogger<dataCollected> _logger;

        public dataCollected(ILogger<dataCollected> logger)
        {
            _logger = logger;
        }

        private readonly PasswordHasher<string> passwordHasher = new();
        public string personal_Information(
            string FirstName,
            string LastName,
            int Age,
            string Email,
            string Password)
        {
            try
            {
                string connection =
                    @"Data Source=(localdb)\project;Database=project";

                using (SqlConnection connect = new SqlConnection(connection))
                {
                    connect.Open();

                    string query = @"
                        INSERT INTO dbo.Users
                            (FirstName, LastName, Age, Email, [Password])
                        VALUES
                            (@FirstName, @LastName, @Age, @Email, @Password)";

                    using (SqlCommand run_query =
                           new SqlCommand(query, connect))
                    {
                        run_query.Parameters.AddWithValue("@FirstName", FirstName);
                        run_query.Parameters.AddWithValue("@LastName", LastName);
                        run_query.Parameters.AddWithValue("@Age", Age);
                        run_query.Parameters.AddWithValue("@Email", Email);

                        string hashedPassword = passwordHasher.HashPassword(
                            Email, Password);

                        run_query.Parameters.AddWithValue("@Password", hashedPassword);

                        int result = run_query.ExecuteNonQuery();

                        if (result > 0)
                        {
                            return "User account created successfully";
                        }

                        return "Fail to create user account";
                    }
                }
            }
            catch (SqlException error)
     when (error.Number == 2627 || error.Number == 2601)
            {
                _logger.LogWarning(
                    "Registration attempted with an email that already exists.");

                return "An account with this email already exists.";
            }
            catch (Exception error)
            {
                _logger.LogError(
                    error,
                    "Failed to create a user account.");

                return "Unable to create user account. Please try again.";
            }
        }
    }
}
