using Microsoft.Extensions.DependencyInjection;

namespace ZooApp;

class App
{
    static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        var container = serviceCollection.BuildServiceProvider();

        var park = container.GetService<Zoo>() ?? throw new InvalidOperationException("Зоопарк не найден.");
        var clinic = container.GetService<IClinic>() ??
                     throw new InvalidOperationException("Ветеринарная клиника не найдена.");

        SeedInventory(park);

        while (true)
        {
            DisplayMenu();
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    AddNewAnimal(park);
                    break;
                case "2":
                    DisplayReport(park);
                    break;
                case "3":
                    ListContactEligibleAnimals(park);
                    break;
                case "4":
                    DisplayInventory(park);
                    break;
                case "5":
                    Console.WriteLine("Выход из программы.");
                    return;
                default:
                    Console.WriteLine("Неверный ввод. Попробуйте снова.");
                    break;
            }
        }
    }

    static void ConfigureServices(ServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<Zoo>();
        serviceCollection.AddSingleton<IClinic, VetClinic>();
    }

    static void SeedInventory(Zoo park)
    {
        park.RegisterItem(new Table("Стол"));
        park.RegisterItem(new Computer("Компьютер"));
    }

    static void DisplayMenu()
    {
        Console.WriteLine("1: Добавить животное, 2: Отчет, 3: Контактный зоопарк, 4: Инвентарь + Животные, 5: Выход");
        Console.Write("Введите номер операции: ");
    }

    static void AddNewAnimal(Zoo park)
    {
        Console.WriteLine("Тип животного (Rabbit, Monkey, Tiger, Wolf):");
        var animalType = Console.ReadLine();
        Console.WriteLine("Имя:");
        var animalName = Console.ReadLine();
        Console.WriteLine("Потребление еды (кг):");

        if (!int.TryParse(Console.ReadLine(), out var foodAmount))
        {
            Console.WriteLine("Ошибка ввода количества еды.");
            return;
        }

        // int random_number = new Random().Next(10000, 99999);
        Animal? newAnimal = CreateAnimal(animalType, animalName, foodAmount);

        if (newAnimal == null)
        {
            Console.WriteLine("Неверный тип животного.");
            return;
        }

        if (park.RegisterAnimal(newAnimal))
        {
            Console.WriteLine("Животное добавлено в зоопарк");
        }
        else
        {
            Console.WriteLine("Ветеринарная клиника не допустила добавление животного.");
        }
    }

    static Animal? CreateAnimal(string? animalType, string? name, int food)
    {
        if (name == null)
        {
            return null;
        }
        
        switch (animalType?.ToLower())
        {
            case "rabbit":
            case "monkey":
                Console.WriteLine("Уровень доброты:");
                if (!int.TryParse(Console.ReadLine(), out var kindness))
                {
                    Console.WriteLine("Ошибка ввода для Уровня Доброты.");
                    return null;
                }

                if (animalType.Equals("rabbit", StringComparison.OrdinalIgnoreCase))
                    return new Rabbit(name, food, kindness);

                if (animalType.Equals("monkey", StringComparison.OrdinalIgnoreCase))
                    return new Monkey(name, food, kindness);

                break;

            case "tiger":
                return new Tiger(name, food);

            case "wolf":
                return new Wolf(name, food);
        }

        return null;
    }

    static void DisplayReport(Zoo park)
    {
        Console.WriteLine($"Животных: {park.CountAnimals()}");
        Console.WriteLine($"Потребление еды: {park.GetTotalFood()} кг");
    }

    static void ListContactEligibleAnimals(Zoo park)
    {
        var eligibleAnimals = park.GetContactEligible();
        if (!eligibleAnimals.Any())
        {
            Console.WriteLine("Нет животных, подходящих для контактного зоопарка.");
            return;
        }

        foreach (var animal in eligibleAnimals)
        {
            Console.WriteLine($"{animal.GetType().ToString().Split(".")[1]} - {animal.Name} - № {animal.Number}");
        }
    }

    static void DisplayInventory(Zoo park)
    {
        foreach (var item in park.Items)
        {
            Console.WriteLine($"{item.Name} - инв. № {item.Number}");
        }
        
        foreach (var animal in park.AnimalList)
        {
            Console.WriteLine($"{animal.GetType().ToString().Split(".")[1]} - {animal.Name} - № {animal.Number}");
        }
    }
}