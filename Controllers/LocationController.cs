
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace project.Controllers
{
    public class LocationRequest
    {
        public string LocationName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }

    [Route("API/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly string connection =
            @"Data Source=(localdb)\project;Database=project;";


        [HttpGet]
        public IActionResult GetLocation()
        {
            List<object> locations = new List<object>();

            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "SELECT * FROM Location";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    connect.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var location = new Dictionary<string, object?>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                location[reader.GetName(i)] =
                                    reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }

                            locations.Add(location);
                        }
                    }
                }
            }

            return Ok(locations);
        }


        // POST: API/Location
        [HttpPost]
        public IActionResult AddLocation(
            [FromBody] LocationRequest location)
        {
            if (string.IsNullOrWhiteSpace(location.LocationName))
            {
                return BadRequest("Location name is required.");
            }

            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = @"
                    INSERT INTO Location (LocationName, Address)
                    VALUES (@LocationName, @Address)";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue(
                        "@LocationName", location.LocationName);

                    command.Parameters.AddWithValue(
                        "@Address", location.Address);

                    connect.Open();
                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        return Ok("Location added successfully.");
                    }

                    return BadRequest("Location was not added.");
                }
            }
        }

        // PUT: API/Location/1
        [HttpPut("{id}")]
        public IActionResult UpdateLocation(int id, [FromBody] LocationRequest location)
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = @"
            UPDATE Location
            SET LocationName = @LocationName,
                Address = @Address
            WHERE LocationID = @ID";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@LocationName", location.LocationName);
                    command.Parameters.AddWithValue("@Address", location.Address);

                    connect.Open();
                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                        return Ok("Location updated successfully.");

                    return NotFound("Location not found.");
                }
            }
        }
        // DELETE: API/Location/1
        [HttpDelete("{id}")]
        public IActionResult DeleteLocation(int id)
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "DELETE FROM Location WHERE LocationID = @ID";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    connect.Open();
                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                        return Ok("Location deleted successfully.");

                    return NotFound("Location not found.");
                }
            }
        }
    }
}