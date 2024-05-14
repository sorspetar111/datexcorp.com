
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

public class WarehouseContext : DbContext
{
    public DbSet<Pallet> Pallets { get; set; }
    public DbSet<Box> Boxes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("your_connection_string_here");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Do not forget to recreate it when database scructure is update/created.
        
        /*
        modelBuilder.Entity<Box>()
            .HasOne(b => b.Pallet)
            .WithMany(p => p.Boxes)
            .HasForeignKey(b => b.PalletId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Box>()
            .HasOne(b => b.ParentBox)
            .WithMany(b => b.ChildrenBoxes)
            .HasForeignKey(b => b.ParentBoxId)
            .OnDelete(DeleteBehavior.Restrict); 
        */
              
        modelBuilder.Entity<Box>()
            .HasOne(b => b.ParentBox)
            .WithMany(b => b.ChildrenBoxes)
            .HasForeignKey(b => b.ParentBoxId)
            .OnDelete(DeleteBehavior.Cascade); // commend this line if you wan to use WarehouseManagerInternalCascade 

            
    }
}


public class WarehouseManager
{
    private readonly WarehouseContext _context;

    public WarehouseManager(WarehouseContext context)
    {
        _context = context;
    }

    public void TakeBox(Box box)
    {
        // We no longer need recurisive due to DeleteBehavior.Cascade
        _context.Remove(box);
        _context.SaveChanges();     
    }
}

[Obsolete("Use WarehouseManager if you want to give entire control to EF. OTHERWISE delete OnDelete Behavior and due it with the recursive code.")]
public class WarehouseManagerCoreInternalCascade: WarehouseManager
{
    private readonly WarehouseContext _context;

    public WarehouseManager(WarehouseContext context)
    {
        _context = context;
    }

    public void TakeBox(Box box)
    {
        // Retrieve the box with its children from the context
        var boxWithChildren = _context.Boxes.Include(b => b.ChildrenBoxes).SingleOrDefault(b => b.Id == box.Id);

        if (boxWithChildren != null)
        {
            // Remove the box and all its children
            _context.Boxes.RemoveRange(GetBoxAndChildren(boxWithChildren));

            // Save changes to the database
            _context.SaveChanges();
        }
    }

    // Helper method to get a box and all its children recursively
    private List<Box> RecursiveGetBoxAndChildren(Box box)
    {
        var boxes = new List<Box> { box };

        foreach (var childBox in box.ChildrenBoxes)
        {
            boxes.AddRange(RecursiveGetBoxAndChildren(childBox));
        }

        return boxes;
    }

 
}


[Obsolete("Use WarehouseManager if you want to give entire control to EF. OTHERWISE delete OnDelete Behavior and due it with the recursive code.")]
public class WarehouseManagerInternalCascade : WarehouseManager
{

    public WarehouseManagerInternalCascade(WarehouseContext context) : base(context)
    {
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
        var childBoxes = _context.Boxes.Where(b => b.ParentBoxId == parentBox.Id).ToList();
        foreach (var childBox in childBoxes)
        {
            boxList.Add(childBox);
            FetchChildBoxes(childBox, boxList);
        }
    }

    public void DeleteBoxes(List<Box> boxList)
    {
        _context.RemoveRange(boxList);
        _context.SaveChanges();
    }

}


