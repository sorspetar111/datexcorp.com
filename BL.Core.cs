

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
            .OnDelete(DeleteBehavior.Cascade); 
    }
}



using System.Collections.Generic;
using System.Linq;

public class WarehouseManager
{
    private readonly WarehouseContext _context;

    public WarehouseManager(WarehouseContext context)
    {
        _context = context;
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
