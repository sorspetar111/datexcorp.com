
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

[TestFixture]
public class WarehouseManagerTests
{
    private WarehouseContext _context;

    [SetUp]
    public void Setup()
    {
        _context = new WarehouseContext();
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

        // Create the manager instance
        WarehouseManager manager = new WarehouseManager(_context);

        // Perform the test
        List<Box> result = manager.RecursiveTakeBox(box);

        // Assert the result
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("BC1", result[0].Barcode); // Assuming the top-level box is added to the list
    }

    [Test]
    public void DeleteBoxes_Deletes_CorrectBoxes()
    {
        // Mock a list of boxes
        List<Box> boxes = _context.Boxes.ToList(); // Get all boxes from the context

        // Create the manager instance
        WarehouseManager manager = new WarehouseManager(_context);

        // Delete the boxes
        manager.DeleteBoxes(boxes);

        // Verify that the boxes are deleted
        Assert.AreEqual(0, _context.Boxes.Count()); // Ensure all boxes are deleted
    }
}
