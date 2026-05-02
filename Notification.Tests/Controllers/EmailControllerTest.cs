using Moq;
using Zuhid.Notification.Controllers;
using Zuhid.Notification.Shared;
using Zuhid.Notification.Welcome;

namespace Zuhid.Notification.Tests.Controllers;

public class EmailControllerTest
{
    [Fact]
    public async Task Welcome_ShouldQueueMessage()
    {
        // Arrange
        var mockQueue = new Mock<NotificationQueue>();
        var controller = new EmailController(mockQueue.Object);
        var message = new WelcomeMessage { CustomerId = Guid.NewGuid() };

        // Act
        await controller.Welcome(message);

        // Assert
        mockQueue.Verify(q => q.QueueMessage(It.Is<WelcomeMessage>(m => m.CustomerId == message.CustomerId)), Times.Once);
    }
}
