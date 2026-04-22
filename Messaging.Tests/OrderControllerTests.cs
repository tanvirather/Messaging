using Zuhid.Messaging;
using Zuhid.Messaging.Controllers;
using Zuhid.Messaging.Mappers;
using Zuhid.Messaging.Repositories;
using Zuhid.Messaging.Models;
using Moq;
using Xunit;

namespace Zuhid.Messaging.Tests;

public class OrderControllerTests
{
    private readonly Mock<IEmailService> _mockEmailService = new();
    private readonly Mock<IOrderMapper> _mockOrderMapper = new();
    private readonly Mock<OrderRepository> _mockOrderRepository = new Mock<OrderRepository>(null!);
    private readonly OrderController _controller;

    public OrderControllerTests()
    {
        _controller = new OrderController(_mockEmailService.Object, _mockOrderMapper.Object, _mockOrderRepository.Object);
    }

    [Fact]
    public async Task Send_Returns_False_When_Order_Not_Found()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _mockOrderRepository.Setup(x => x.GetOrderWithDetailsAsync(orderId))
            .ReturnsAsync((OrderModel?)null);

        // Act
        var result = await _controller.Send(orderId);

        // Assert
        Assert.False(result);
        _mockEmailService.Verify(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Send_Uses_Customer_Email_When_Available()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new OrderModel
        {
            Id = orderId,
            Number = "123",
            Customer = new CustomerModel { Email = "customer@test.com" }
        };
        var subject = "Subject";
        var body = "Body";

        _mockOrderRepository.Setup(x => x.GetOrderWithDetailsAsync(orderId))
            .ReturnsAsync(order);
        _mockOrderMapper.Setup(x => x.Map(order))
            .ReturnsAsync((subject, body));
        _mockEmailService.Setup(x => x.SendEmailAsync("customer@test.com", subject, body))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Send(orderId);

        // Assert
        Assert.True(result);
        _mockEmailService.Verify(x => x.SendEmailAsync("customer@test.com", subject, body), Times.Once);
    }

    [Fact]
    public async Task Send_Uses_Default_Email_When_Customer_Email_Is_Null()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new OrderModel
        {
            Id = orderId,
            Number = "123",
            Customer = null
        };
        var subject = "Subject";
        var body = "Body";

        _mockOrderRepository.Setup(x => x.GetOrderWithDetailsAsync(orderId))
            .ReturnsAsync(order);
        _mockOrderMapper.Setup(x => x.Map(order))
            .ReturnsAsync((subject, body));
        _mockEmailService.Setup(x => x.SendEmailAsync("customer@example.com", subject, body))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Send(orderId);

        // Assert
        Assert.True(result);
        _mockEmailService.Verify(x => x.SendEmailAsync("customer@example.com", subject, body), Times.Once);
    }
}
