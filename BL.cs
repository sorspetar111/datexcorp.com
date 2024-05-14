
// This version do not use cascade deletion. This is just some kind of recursive internal deletion with clasic ORM.

public class WarehouseManager
{
    private readonly string _connectionString;

    public WarehouseManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Box> RecursiveTakeBox(Box box)
    {
        List<Box> boxList = new List<Box>();
        boxList.Add(box);

        FetchChildBoxes(box, boxList);

        DeleteBoxes(boxList);

        return boxList;
    }

    private void FetchChildBoxes(Box parentBox, List<Box> boxList)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            string query = "SELECT id, barcode FROM Box WHERE parent_box_id = @parentBoxId";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@parentBoxId", parentBox.Id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Box childBox = new Box
                        {
                            Id = (int)reader["id"],
                            Barcode = reader["barcode"].ToString(),
                            ParentBoxId = parentBox.Id
                        };
                        boxList.Add(childBox);
                        FetchChildBoxes(childBox, boxList);
                    }
                }
            }
        }
    }

    public void DeleteBoxes(List<Box> boxList)
    {
        // Sort the list of boxes based on their hierarchy
        var sortedBoxList = boxList.OrderByDescending(b => b.ParentBoxId.HasValue).ThenBy(b => b.ParentBoxId).ToList();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            foreach (var box in boxList)
            {
                // Delete the box from the database
                DeleteBox(connection, box);
            }
        }
    }

    private void DeleteBox(SqlConnection connection, Box box)
    {
        // Delete the box from the database
        string deleteQuery = "DELETE FROM Box WHERE id = @boxId";
        using (SqlCommand command = new SqlCommand(deleteQuery, connection))
        {
            command.Parameters.AddWithValue("@boxId", box.Id);
            command.ExecuteNonQuery();
        }
    }


}

