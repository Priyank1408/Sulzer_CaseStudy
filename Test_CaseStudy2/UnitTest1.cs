using CaseStudy2;
namespace Test_CaseStudy2
{
    public class UnitTest1
    {
        [Fact]
        public void BasicExample_ReturnsCorrectTotalPrice()
        {
            // Arrange
            var order = new List<Item> {
                new Item("Laptop", 1, 1000.00m),
                new Item("Mouse", 3, 25.00m),
                new Item("Keyboard", 2, 50.00m)
            };  
            decimal expectedPrice = 1109.125m;

            // Act
            decimal actualPrice = Program.CalculateTotalPrice(order);

            // Assert
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void NoItems_ReturnsZeroTotalPrice()
        {
            // Arrange
            var order = new List<Item>();
            decimal expectedPrice = 0;

            // Act
            decimal actualPrice = Program.CalculateTotalPrice(order);

            // Assert
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void AllItemsQualifyForDiscount_ReturnsCorrectTotalPrice()
        {
            // Arrange
            var order = new List<Item> {
                new Item("Item1", 3, 50.00m),
                new Item("Item2", 5, 20.00m)
            };
            decimal expectedPrice = (50.00m * 3 * 0.9m) + (20.00m * 5 * 0.9m);
            expectedPrice *= 0.95m; // Apply 5% discount because total > $100

            // Act
            decimal actualPrice = Program.CalculateTotalPrice(order);

            // Assert
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void TotalExactly100_ReturnsCorrectTotalPrice()
        {
            // Arrange
            var order = new List<Item> {
                new Item("Item1", 2, 50.00m)
            };
            decimal expectedPrice = 100m;

            // Act
            decimal actualPrice = Program.CalculateTotalPrice(order);

            // Assert
            Assert.Equal(expectedPrice, actualPrice);
        }
    }
}