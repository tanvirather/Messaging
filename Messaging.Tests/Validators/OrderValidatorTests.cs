using Microsoft.Extensions.Logging;
using Moq;
using Zuhid.Messaging.Messages;
using Zuhid.Messaging.Models;
using Zuhid.Messaging.Validators;

namespace Zuhid.Messaging.Tests.Validators;

public class OrderValidatorTests
{
    private readonly Mock<ILogger<OrderValidator>> _mockLogger = new();
    private readonly OrderValidator _validator;

    public OrderValidatorTests()
    {
        _validator = new OrderValidator(_mockLogger.Object);
    }

    [Fact]
    public void IsValid_ReturnsTrue_WhenOrderAndCustomerExist()
    {
        // Arrange
        var message = new OrderMessage { OrderId = Guid.NewGuid() };
        var order = new OrderModel { Id = message.OrderId, Customer = new CustomerModel { Email = "test@example.com" } };

        // Act
        var result = _validator.IsValid(message, order);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_ReturnsFalse_AndLogsWarning_WhenOrderIsNull()
    {
        // Arrange
        var message = new OrderMessage { OrderId = Guid.NewGuid() };

        // Act
        var result = _validator.IsValid(message, null);

        // Assert
        Assert.False(result);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void IsValid_ReturnsFalse_AndLogsWarning_WhenCustomerIsNull()
    {
        // Arrange
        var message = new OrderMessage { OrderId = Guid.NewGuid() };
        var order = new OrderModel { Id = message.OrderId, Customer = null };

        // Act
        var result = _validator.IsValid(message, order);

        // Assert
        Assert.False(result);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Customer for")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
