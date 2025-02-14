using Moq;
using ZooApp;

namespace ZooAppTests
{
    [TestFixture]
    public class ZooTests
    {
        [Test]
        public void Can_Register_Multiple_Animals()
        {
            // Arrange
            var mockConsole = new Mock<TextReader>();
            mockConsole.SetupSequence(x => x.ReadLine())
                .Returns("y")
                .Returns("y");
            Console.SetIn(mockConsole.Object);
            var vetClinic = new VetClinic();
            var zoo = new Zoo(vetClinic);
            var tiger = new Tiger("Simba", 20);
            var rabbit = new Rabbit("Bumba", 3, 8);

            // Act
            zoo.RegisterAnimal(tiger);
            zoo.RegisterAnimal(rabbit);

            // Assert
            Assert.That(zoo.AnimalList.Count, Is.EqualTo(2));
            Assert.That(zoo.AnimalList.Any(a => a == tiger), Is.True);
            Assert.That(zoo.AnimalList.Any(a => a == rabbit), Is.True);
        }

        [Test]
        public void Can_Register_Multiple_Inventory_Items()
        {
            // Arrange
            var vetClinic = new VetClinic();
            var zoo = new Zoo(vetClinic);
            var desk = new Table("Office Desk");
            var pc = new Computer("Director PC");

            // Act
            zoo.RegisterItem(desk);
            zoo.RegisterItem(pc);

            // Assert
            Assert.That(zoo.Items.Count, Is.EqualTo(2));
            Assert.That(zoo.Items.Any(i => i == desk), Is.True);
            Assert.That(zoo.Items.Any(i => i == pc), Is.True);
        }

        [Test]
        public void Does_Not_Allow_Empty_Animal_Registration()
        {
            // Arrange
            var mockConsole = new Mock<TextReader>();
            mockConsole.SetupSequence(x => x.ReadLine())
                .Returns("y")
                .Returns("y");
            Console.SetIn(mockConsole.Object);
            var vetClinic = new VetClinic();
            var zoo = new Zoo(vetClinic);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => zoo.RegisterAnimal(null));
            Assert.That(ex.Message, Is.EqualTo("Animal cannot be null. (Parameter 'a')"));
        }

        [Test]
        public void Does_Not_Allow_Null_Inventory_Registration()
        {
            // Arrange
            var vetClinic = new VetClinic();
            var zoo = new Zoo(vetClinic);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => zoo.RegisterItem(null));
            Assert.That(ex.Message, Is.EqualTo("Inventory item cannot be null. (Parameter 'i')"));
        }

        [Test]
        public void Can_Get_Food_Consumption_For_Empty_Zoo()
        {
            // Arrange
            var vetClinic = new VetClinic();
            var zoo = new Zoo(vetClinic);

            // Act
            int totalFood = zoo.GetTotalFood();

            // Assert
            Assert.That(totalFood, Is.EqualTo(0));
        }

        [Test]
        public void Contact_Eligible_Filter_Returns_Empty_For_No_Animals()
        {
            // Arrange
            var vetClinic = new VetClinic();
            var zoo = new Zoo(vetClinic);

            // Act
            var eligibleAnimals = zoo.GetContactEligible();

            // Assert
            Assert.That(eligibleAnimals, Is.Empty);
        }

        [Test]
        public void Can_Calculate_Total_Food_Accurately()
        {
            // Arrange
            var mockConsole = new Mock<TextReader>();
            mockConsole.SetupSequence(x => x.ReadLine())
                .Returns("y")
                .Returns("y");
            Console.SetIn(mockConsole.Object);
            var vetClinic = new VetClinic();
            var zoo = new Zoo(vetClinic);
            zoo.RegisterAnimal(new Tiger("Mufasa", 15));
            zoo.RegisterAnimal(new Rabbit("Bugs", 8, 12));

            // Act
            int totalFood = zoo.GetTotalFood();

            // Assert
            Assert.That(totalFood, Is.EqualTo(23)); // 15 + 8
        }

        [Test]
        public void Contact_Eligible_Filter_Returns_Correct_Animals()
        {
            // Arrange
            var mockConsole = new Mock<TextReader>();
            mockConsole.SetupSequence(x => x.ReadLine())
                .Returns("y")
                .Returns("y");
            Console.SetIn(mockConsole.Object);
            var vetClinic = new VetClinic();
            var zoo = new Zoo(vetClinic);
            var kindRabbit = new Rabbit("Good Rabbit", 4, 7); // Kindness > 5
            var unfriendlyRabbit = new Rabbit("Bad Rabbit", 3, 3); // Kindness <= 5

            zoo.RegisterAnimal(kindRabbit);
            zoo.RegisterAnimal(unfriendlyRabbit);

            // Act
            var eligibleAnimals = zoo.GetContactEligible();

            // Assert
            Assert.That(eligibleAnimals.Count(), Is.EqualTo(1));
            Assert.That(eligibleAnimals.First().Name, Is.EqualTo(kindRabbit.Name));
        }

        [TestCase("y", true)]
        [TestCase("n", false)]
        public void VetClinic_AssessHealth_Returns_Correct_Values(string input, bool expectedResult)
        {
            // Arrange
            var mockConsole = new Mock<TextReader>();
            mockConsole.Setup(x => x.ReadLine()).Returns(input);
            Console.SetIn(mockConsole.Object);
            var vetClinic = new VetClinic();

            var animal = new Tiger("Scar", 12);

            // Act
            var result = vetClinic.AssessHealth(animal);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Zoo_Contains_No_Items_At_Initialization()
        {
            // Arrange
            var vetClinic = new VetClinic();
            var zoo = new Zoo(vetClinic);

            // Act
            int animalCount = zoo.AnimalList.Count;
            int inventoryCount = zoo.Items.Count;

            // Assert
            Assert.That(animalCount, Is.EqualTo(0));
            Assert.That(inventoryCount, Is.EqualTo(0));
        }
    }
}