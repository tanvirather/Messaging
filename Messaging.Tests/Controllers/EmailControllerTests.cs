using Microsoft.AspNetCore.Mvc;
using Moq;
using Zuhid.Messaging.Controllers;
using Zuhid.Messaging.Messages;

namespace Zuhid.Messaging.Tests.Controllers;

public class EmailControllerTests
{
    private readonly Mock<MessagingQueue> _mockEmailQueue = new();
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
}
