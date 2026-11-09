
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace project.Controllers
{
    public class CategoryRequest
    {
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    [Route("API/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly string connection =
            @"Data Source=(localdb)\project;Database=project;";



        [HttpGet]
        public IActionResult GetCategories()
        {
            List<object> categories = new List<object>();

            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "SELECT * FROM Categories";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    connect.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var category = new Dictionary<string, object?>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                category[reader.GetName(i)] =
                                    reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }

                            categories.Add(category);
                        }
                    }
                }
            }

            return Ok(categories);
        }



        // POST: API/Categories
        [HttpPost]
        public IActionResult AddCategory(
            [FromBody] CategoryRequest category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                return BadRequest("Category name is required.");
            }

            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = @"
                    INSERT INTO Categories (CategoryName, Description)
                    VALUES (@CategoryName, @Description)";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue(
                        "@CategoryName", category.CategoryName);

                    command.Parameters.AddWithValue(
                        "@Description", category.Description);

                    connect.Open();
                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        return Ok("Category added successfully.");
                    }

                    return BadRequest("Category was not added.");
                }
            }
        }

        // PUT: API/Categories/1
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryRequest category)
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = @"UPDATE Categories
                         SET CategoryName = @Name,
                             Description = @Description
                         WHERE CategoryID = @ID";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@Name", category.CategoryName);
                    command.Parameters.AddWithValue("@Description", category.Description);

                    connect.Open();
                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                        return Ok("Category updated successfully.");

                    return NotFound("Category not found.");
                }
            }
        }

        // DELETE: API/Categories/1
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "DELETE FROM Categories WHERE CategoryID = @ID";

                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    connect.Open();

                    int rows = command.ExecuteNonQuery();

                    if (rows > 0)
                        return Ok("Category deleted successfully.");

                    return NotFound("Category not found.");
                }
            }
        }
    }
}