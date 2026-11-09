
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace project.Controllers
{
    public class ResultRequest
    {
        public int EnrollmentID { get; set; }
        public int Position { get; set; }
    }

    [Route("API/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly string connection =
            @"Data Source=(localdb)\project;Database=project;";


        [HttpGet]
        public IActionResult GetResults()
        {
            List<object> results = new List<object>();

            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "SELECT * FROM Results";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    connect.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var result = new Dictionary<string, object?>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                result[reader.GetName(i)] =
                                    reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }

                            results.Add(result);
                        }
                    }
                }
            }

            return Ok(results);
        }

        // POST: API/Results
        [HttpPost]
        public IActionResult AddResult([FromBody] ResultRequest result)
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = @"
                    INSERT INTO Results (EnrollmentID, Position)
                    VALUES (@EnrollmentID, @Position)";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue(
                        "@EnrollmentID", result.EnrollmentID);

                    command.Parameters.AddWithValue(
                        "@Position", result.Position);

                    connect.Open();
                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        return Ok("Result added successfully.");
                    }

                    return BadRequest("Result was not added.");
                }
            }
        }

        // PUT: API/Results/1
        [HttpPut("{id}")]
        public IActionResult UpdateResult(int id, [FromBody] ResultRequest result)
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = @"
                    UPDATE Results
                    SET EnrollmentID = @EnrollmentID,
                        Position = @Position
                    WHERE ResultID = @ID";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@EnrollmentID", result.EnrollmentID);
                    command.Parameters.AddWithValue("@Position", result.Position);

                    connect.Open();
                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        return Ok("Result updated successfully.");
                    }

                    return NotFound("Result not found.");
                }
            }
        }

        // DELETE: API/Results/1
        [HttpDelete("{id}")]
        public IActionResult DeleteResult(int id)
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "DELETE FROM Results WHERE ResultID = @ID";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    connect.Open();
                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        return Ok("Result deleted successfully.");
                    }

                    return NotFound("Result not found.");
                }
            }
        }

    }
}