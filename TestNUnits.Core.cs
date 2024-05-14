
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
        List<Box> boxes = _context.Boxes.ToList(); 
      
        WarehouseManager manager = new WarehouseManager(_context);

        // Delete the boxes
        // manager.DeleteBoxes(boxes);
         manager.DeleteBox(boxes.FirstOrDefault());
       
        Assert.AreEqual(0, _context.Boxes.Count()); 
    }
}
