namespace ZooApp;

public class VetClinic : IClinic
{
    public bool AssessHealth(Animal a)
    {
        Console.WriteLine($"Осмотр животного {a.Name}. Здоров? (y/n):");
        var input = Console.ReadLine()?.ToLower();
        return input == "y";
    }
}
