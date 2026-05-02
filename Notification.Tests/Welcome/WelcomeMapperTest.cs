using Zuhid.Notification.Welcome;

namespace Zuhid.Notification.Tests.Welcome;

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
        var mailMessage = await composer.Map(model);


        // Assert
        Assert.Equal("Welcome to Our Platform!", mailMessage.Subject);
        Assert.Contains("Welcome, John!", mailMessage.Body);
        Assert.Contains("Name:</strong> John Doe", mailMessage.Body);
        Assert.Contains("john.doe@example.com", mailMessage.Body);
        Assert.Contains("1234567890", mailMessage.Body);
        Assert.Contains("123 Main St", mailMessage.Body);
        Assert.Contains("New York", mailMessage.Body);
        Assert.Contains("NY", mailMessage.Body);
        Assert.Contains("10001", mailMessage.Body);
        Assert.Contains("USA", mailMessage.Body);
        Assert.DoesNotContain("{{", mailMessage.Body);
        Assert.DoesNotContain("}}", mailMessage.Body);
        Assert.True(mailMessage.IsBodyHtml);
    }
}
