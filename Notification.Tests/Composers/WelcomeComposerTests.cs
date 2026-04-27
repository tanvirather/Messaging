using Zuhid.Notification.Composers;
using Zuhid.Notification.Models;

namespace Zuhid.Notification.Tests.Composers;

public class WelcomeComposerTests
{
    private readonly WelcomeComposer _composer = new();

    [Fact]
    public async Task Map_Returns_Correct_Subject_And_Body()
    {
        // Arrange
        var welcome = new WelcomeModel
        {
            CustomerId = Guid.NewGuid(),
            Customer = new CustomerModel
            {
                Email = "john.doe@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "1234567890"
            },
            DefaultAddress = new AddressModel
            {
                Street = "123 Main St",
                City = "Anytown",
                State = "CA",
                ZipCode = "12345",
                Country = "USA"
            }
        };

        // Act
        var (subject, body) = await _composer.Map(welcome);

        // Assert
        Assert.Equal("Welcome to Our Platform!", subject);
        Assert.Contains("Welcome, John!", body);
        Assert.Contains("John Doe", body);
        Assert.Contains("john.doe@example.com", body);
        Assert.Contains("1234567890", body);
        Assert.Contains("123 Main St", body);
        Assert.Contains("Anytown, CA 12345", body);
        Assert.Contains("USA", body);
        Assert.Contains("<html>", body);
    }

    [Fact]
    public async Task Map_Handles_Missing_Address()
    {
        // Arrange
        var welcome = new WelcomeModel
        {
            CustomerId = Guid.NewGuid(),
            Customer = new CustomerModel
            {
                Email = "jane.doe@example.com",
                FirstName = "Jane",
                LastName = "Doe"
            },
            DefaultAddress = null
        };

        // Act
        var (subject, body) = await _composer.Map(welcome);

        // Assert
        Assert.Equal("Welcome to Our Platform!", subject);
        Assert.Contains("Welcome, Jane!", body);
        Assert.Contains("Not provided", body); // Phone number default
    }
}
