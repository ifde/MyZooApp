namespace ZooApp;

public class Zoo
{
    private readonly IClinic _vetClinic;
    public List<Animal> AnimalList { get; set; } = new List<Animal>();
    public List<Thing> Items { get; set; } = new List<Thing>();
    
    private int _cnt = 0; // уникальный счетчик номера животного / инвентаря
    
    public Zoo(IClinic vetClinic)
    {
        _vetClinic = vetClinic;
    }

    public bool AssessAnimal(Animal a)
    {
        if (_vetClinic.AssessHealth(a))
        {
            AnimalList.Add(a);
            return true;
        }
        return false;
    }
    
    public bool RegisterAnimal(Animal a)
    {
        if (a == null)
        {
            throw new ArgumentNullException(nameof(a), "Animal cannot be null.");
        }
        
        if (_vetClinic.AssessHealth(a))
        {
            a.Number = _cnt;
            _cnt++;
            AnimalList.Add(a);
            return true;
        }
        return false;
    }
    
    public void RegisterItem(Thing i)
    {
        if (i == null) 
            throw new ArgumentNullException(nameof(i), "Inventory item cannot be null.");
        i.Number = _cnt;
        _cnt++;
        Items.Add(i);
    }

    public int GetTotalFood() => AnimalList.Sum(x => x.Food);

    public int CountAnimals() => AnimalList.Count;

    public IEnumerable<Animal> GetContactEligible() =>
        AnimalList.OfType<Herbo>().Where(x => x.Kindness > 5);
}