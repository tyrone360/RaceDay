
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace project.Controllers
{
    public class EnrollmentRequest
    {
        public int UserID { get; set; }
        public int EventID { get; set; }
    }

    [Route("API/[controller]")]
    [ApiController]
    public class EventEnrollmentController : ControllerBase
    {
        private readonly string connection =
            @"Data Source=(localdb)\project;Database=project;";


        [HttpGet]
        public IActionResult GetEventEnrollment()
        {
            List<object> enrollments = new List<object>();

            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "SELECT * FROM [Event Enrollment]";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    connect.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var enrollment = new Dictionary<string, object?>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                enrollment[reader.GetName(i)] =
                                    reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }

                            enrollments.Add(enrollment);
                        }
                    }
                }
            }

            return Ok(enrollments);
        }


        // POST: API/EventEnrollment
        [HttpPost]
        public IActionResult AddEnrollment(
            [FromBody] EnrollmentRequest enrollment)
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = @"
                    INSERT INTO [Event Enrollment] (UserID, EventID)
                    VALUES (@UserID, @EventID)";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue(
                        "@UserID", enrollment.UserID);

                    command.Parameters.AddWithValue(
                        "@EventID", enrollment.EventID);

                    connect.Open();
                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        return Ok("Enrollment added successfully.");
                    }

                    return BadRequest("Enrollment was not added.");
                }
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "DELETE FROM [Event Enrollment] WHERE EnrollmentID = @ID";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    try
                    {
                        connect.Open();
                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                            return Ok("Enrollment deleted successfully.");

                        return NotFound("Enrollment not found.");
                    }
                    catch (SqlException ex) when (ex.Number == 547)
                    {
                        return Conflict("This enrollment is linked to another record and cannot be deleted.");
                    }
                }
            }
        }
    }
}