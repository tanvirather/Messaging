using Zuhid.Notification.Welcome;

namespace Zuhid.Notification.Tests.Welcome
{
    public class WelcomeMapperTest
    {
        [Fact]
        public async Task Map_ShouldReturnExpectedSubjectAndBody()
        {
            // Arrange
            var composer = new WelcomeMapper();
            var model = new WelcomeModel
            {
                Customer = new CustomerModel
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "1234567890"
                },
                DefaultAddress = new AddressModel
                {
                    Street = "123 Main St",
                    City = "New York",
                    State = "NY",
                    ZipCode = "10001",
                    Country = "USA"
                }
            };

            // Act
            var (subject, body) = await composer.Map(model);


            // Assert
            Assert.Equal("Welcome to Our Platform!", subject);
            Assert.Contains("Welcome, John!", body);
            Assert.Contains("Name:</strong> John Doe", body);
            Assert.Contains("john.doe@example.com", body);
            Assert.Contains("1234567890", body);
            Assert.Contains("123 Main St", body);
            Assert.Contains("New York", body);
            Assert.Contains("NY", body);
            Assert.Contains("10001", body);
            Assert.Contains("USA", body);
            Assert.DoesNotContain("{{", body);
            Assert.DoesNotContain("}}", body);
        }
    }
}
