
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace project.Controllers
{
    public class EventRequest
    {
        public string EventName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
    }

    [Route("API/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly string connection =
            @"Data Source=(localdb)\project;Database=project;";


        [HttpGet]
        public IActionResult GetEvents()
        {
            List<object> events = new List<object>();

            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "SELECT * FROM Events";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    connect.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var eventItem = new Dictionary<string, object?>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                eventItem[reader.GetName(i)] =
                                    reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }

                            events.Add(eventItem);
                        }
                    }
                }
            }

            return Ok(events);
        }


        // POST: API/Events
        [HttpPost]
        public IActionResult AddEvent([FromBody] EventRequest newEvent)
        {
            if (string.IsNullOrWhiteSpace(newEvent.EventName))
            {
                return BadRequest("Event name is required.");
            }

            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = @"
                    INSERT INTO Events (EventName, EventDate)
                    VALUES (@EventName, @EventDate)";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue(
                        "@EventName", newEvent.EventName);

                    command.Parameters.AddWithValue(
                        "@EventDate", newEvent.EventDate);

                    connect.Open();
                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        return Ok("Event added successfully.");
                    }

                    return BadRequest("Event was not added.");
                }
            }
        }

        // PUT: API/Events/1
        [HttpPut("{id}")]
        public IActionResult UpdateEvent(int id, [FromBody] EventRequest newEvent)
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = @"
            UPDATE Events
            SET EventName = @EventName,
                EventDate = @EventDate
            WHERE EventID = @ID";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@EventName", newEvent.EventName);
                    command.Parameters.AddWithValue("@EventDate", newEvent.EventDate);

                    connect.Open();
                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                        return Ok("Event updated successfully.");

                    return NotFound("Event not found.");
                }
            }
        }


        // DELETE: API/Events/1
        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                try
                {
                    string query = "DELETE FROM Events WHERE EventID = @ID";

                    using (SqlCommand command = new SqlCommand(query, connect))
                    {
                        command.Parameters.AddWithValue("@ID", id);

                        connect.Open();
                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                            return Ok("Event deleted successfully.");

                        return NotFound("Event not found.");
                    }
                }
                catch (SqlException ex) when (ex.Number == 547)
                {
                    return Conflict(
                        "This event cannot be deleted because it has related enrollment records.");
                }
            }
        }
    }
}