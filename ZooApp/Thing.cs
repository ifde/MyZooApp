namespace ZooApp;

public abstract class Thing : IInventory
{
    public string? Name { get; set; }
    public int Number { get; set; }
}