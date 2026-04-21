using Zuhid.Messaging;
using Zuhid.Messaging.Controllers.Email;
using Zuhid.Messaging.Mappers.Email;
using Moq;

namespace Zuhid.Messaging.Tests;

public class OrderControllerTests
{
    [Fact]
    public async Task Send_Should_Call_EmailService_With_Mapped_Content()
    {
        // Arrange
        var mockEmailService = new Mock<IEmailService>();
        var mockOrderMapper = new Mock<IOrderMapper>();

        var orderId = Guid.NewGuid();
        var expectedSubject = "Expected Subject";
        var expectedBody = "Expected Body";

        mockOrderMapper.Setup(x => x.MapOrderConfirmationAsync(orderId))
            .ReturnsAsync((expectedSubject, expectedBody));

        var controller = new OrderController(mockEmailService.Object, mockOrderMapper.Object);

        // Act
        var result = await controller.Send(orderId);

        // Assert
        Assert.True(result);
        mockEmailService.Verify(x => x.SendEmailAsync(
            "customer@example.com",
            expectedSubject,
            expectedBody
        ), Times.Once);
    }
}
