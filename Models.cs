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
