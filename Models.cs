// Clasic one version
public class Pallet
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class Box
{
    public int Id { get; set; }
    public string Barcode { get; set; }
    public bool IsOpened { get; set; }
    public int? PalletId { get; set; }
    public int? ParentBoxId { get; set; }
}



// This is EF version of models
public class Pallet
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navigation property
    public ICollection<Box> Boxes { get; set; }
}

public class Box
{
    public int Id { get; set; }
    public string Barcode { get; set; }
    public bool IsOpened { get; set; }
    public int? PalletId { get; set; }
    public int? ParentBoxId { get; set; }
  
    public Box ParentBox { get; set; }
    public Pallet Pallet { get; set; }
    public ICollection<Box> ChildrenBoxes { get; set; }
}
