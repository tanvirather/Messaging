using Microsoft.Extensions.Logging;
using Moq;
using Zuhid.Notification.Messages;
using Zuhid.Notification.Models;
using Zuhid.Notification.Validators;

namespace Zuhid.Notification.Tests.Validators;

public class WelcomeValidatorTests
{
    private readonly Mock<ILogger<WelcomeValidator>> _mockLogger;
    private readonly WelcomeValidator _validator;

    public WelcomeValidatorTests()
    {
        _mockLogger = new Mock<ILogger<WelcomeValidator>>();
        _validator = new WelcomeValidator(_mockLogger.Object);
    }

    [Fact]
    public void IsValid_WhenValid_ReturnsTrue()
    {
        // Arrange
        var message = new WelcomeMessage { CustomerId = Guid.NewGuid() };
        var welcome = new WelcomeModel
        {
            CustomerId = message.CustomerId,
            Customer = new CustomerModel { Email = "test@example.com" }
        };

        // Act
        var result = _validator.IsValid(message, welcome);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_WhenWelcomeIsNull_ReturnsFalse()
    {
        // Arrange
        var message = new WelcomeMessage { CustomerId = Guid.NewGuid() };
        WelcomeModel? welcome = null;

        // Act
        var result = _validator.IsValid(message, welcome);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_WhenCustomerIsNull_ReturnsFalse()
    {
        // Arrange
        var message = new WelcomeMessage { CustomerId = Guid.NewGuid() };
        var welcome = new WelcomeModel
        {
            CustomerId = message.CustomerId,
            Customer = null
        };

        // Act
        var result = _validator.IsValid(message, welcome);

        // Assert
        Assert.False(result);
    }
}
