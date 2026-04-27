using Microsoft.AspNetCore.Mvc;
using Moq;
using Zuhid.Notification.Controllers;
using Zuhid.Notification.Messages;

namespace Zuhid.Notification.Tests.Controllers;

public class EmailControllerTests
{
    private readonly Mock<NotificationQueue> _mockEmailQueue = new();
    private readonly EmailController _controller;

    public EmailControllerTests()
    {
        _controller = new EmailController(_mockEmailQueue.Object);
    }

    [Fact]
    public async Task Order_Returns_Accepted_And_Queues_OrderId()
    {
        // Arrange
        var orderMessage = new OrderMessage { OrderId = Guid.NewGuid() };

        // Act
        var result = await _controller.Order(orderMessage);

        // Assert
        Assert.IsType<AcceptedResult>(result);
        _mockEmailQueue.Verify(x => x.QueueMessage(It.Is<OrderMessage>(m => m.OrderId == orderMessage.OrderId)), Times.Once);
    }

    [Fact]
    public async Task Welcome_Returns_Accepted_And_Queues_CustomerId()
    {
        // Arrange
        var welcomeMessage = new WelcomeMessage { CustomerId = Guid.NewGuid() };

        // Act
        var result = await _controller.Welcome(welcomeMessage);

        // Assert
        Assert.IsType<AcceptedResult>(result);
        _mockEmailQueue.Verify(x => x.QueueMessage(It.Is<WelcomeMessage>(m => m.CustomerId == welcomeMessage.CustomerId)), Times.Once);
    }
}
