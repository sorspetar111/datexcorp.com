
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;

[TestFixture]
public class WarehouseManagerTests
{
    private WarehouseManager _manager;
    private string _connectionString;

    [SetUp]
    public void Setup()
    {
        // Initialize the connection string for testing
        _connectionString = "your_test_connection_string_here";

        // Create the manager instance
        _manager = new WarehouseManager(_connectionString);
    }

    [Test]
    public void RecursiveTakeBox_Returns_CorrectBoxList()
    {
        // Mock a box
        Box box = new Box
        {
            Id = 1,
            Barcode = "BC1",
            ParentBoxId = null // Assume it's a top-level box
        };

        // Perform the test
        List<Box> result = _manager.RecursiveTakeBox(box);

        // Assert the result
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("BC1", result[0].Barcode); // Assuming the top-level box is added to the list
    }

    [Test]
    public void DeleteBoxes_Deletes_CorrectBoxes()
    {
        // Use a transaction to roll back changes made during the test
        using (var scope = new TransactionScope())
        {
            // Mock a list of boxes
            List<Box> boxes = new List<Box>
            {
                new Box { Id = 1, Barcode = "BC1" },
                new Box { Id = 2, Barcode = "BC2" }
                // Add more mock boxes as needed
            };

            // Delete the boxes
            _manager.DeleteBoxes(boxes);

            // Verify that the boxes are deleted by attempting to retrieve them from the database
            foreach (var box in boxes)
            {
                Box deletedBox = GetBoxById(box.Id);
                Assert.IsNull(deletedBox); // Ensure the box is deleted
            }
        }
    }

    private Box GetBoxById(int boxId)
    {
        // Fetch box from the database for verification
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string query = "SELECT * FROM Box WHERE Id = @boxId";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@boxId", boxId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Box
                        {
                            Id = (int)reader["Id"],
                            Barcode = reader["Barcode"].ToString(),
                            ParentBoxId = reader.IsDBNull(reader.GetOrdinal("ParentBoxId")) ? null : (int?)reader["ParentBoxId"]
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}






