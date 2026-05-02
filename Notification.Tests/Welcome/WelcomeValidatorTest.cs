using Microsoft.Extensions.Logging;
using Moq;
using Zuhid.Notification.Shared;
using Zuhid.Notification.Welcome;

namespace Zuhid.Notification.Tests.Welcome;

public class WelcomeValidatorTest
{
    private readonly Mock<ILogger<WelcomeValidator>> _loggerMock;
    private readonly WelcomeValidator _validator;

    public WelcomeValidatorTest()
    {
        _loggerMock = new Mock<ILogger<WelcomeValidator>>();
        _validator = new WelcomeValidator(_loggerMock.Object);
    }

    [Fact]
    public void Validate_WhenWelcomeIsNull_ShouldThrowValidatorExceptionAndLogWarning()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var message = new WelcomeMessage { CustomerId = customerId };
        WelcomeModel? welcome = null;

        // Act & Assert
        var exception = Assert.Throws<ValidatorException>(() => _validator.Validate(message, welcome));

        var expectedMessage = $"Customer {customerId} not found for welcome email.";
        Assert.Contains(expectedMessage, exception.Message);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(expectedMessage)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Once);
    }

    [Fact]
    public void Validate_WhenWelcomeIsNotNull_ShouldNotThrowException()
    {
        // Arrange
        var message = new WelcomeMessage { CustomerId = Guid.NewGuid() };
        var welcome = new WelcomeModel();

        // Act
        var exception = Record.Exception(() => _validator.Validate(message, welcome));

        // Assert
        Assert.Null(exception);
    }
}
