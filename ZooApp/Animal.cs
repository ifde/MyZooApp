namespace ZooApp;

public abstract class Animal : IInventory, IAlive
{
    public string? Name { get; set; }
    public int Food { get; set; }
    
    public int Number { get; set; }
}