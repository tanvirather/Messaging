using Microsoft.Extensions.Logging;
using Moq;
using Zuhid.Notification.Consumers;
using Zuhid.Notification.Composers;
using Zuhid.Notification.Messages;
using Zuhid.Notification.Models;
using Zuhid.Notification.Repositories;
using Zuhid.Notification.Validators;

namespace Zuhid.Notification.Tests.Consumers;

public class OrderConsumerTests
{
    private readonly Mock<EmailService> _mockEmailService;
    private readonly Mock<OrderRepository> _mockRepository;
    private readonly Mock<OrderComposer> _mockComposer;
    private readonly Mock<OrderValidator> _mockValidator;
    private readonly OrderConsumer _consumer;

    public OrderConsumerTests()
    {
        // For classes with constructors, we need to provide mocks or use real instances if they are simple
        // OrderRepository needs NotificationContext. We can use Moq for the repository if its methods are virtual.
        // Let's check OrderRepository structure.

        _mockEmailService = new Mock<EmailService>(null!, null!);
        _mockRepository = new Mock<OrderRepository>(null!); // Passing null for context as we'll mock methods
        _mockComposer = new Mock<OrderComposer>();
        _mockValidator = new Mock<OrderValidator>(new Mock<ILogger<OrderValidator>>().Object);

        _consumer = new OrderConsumer(
            _mockEmailService.Object,
            _mockRepository.Object,
            _mockComposer.Object,
            _mockValidator.Object);
    }

    [Fact]
    public async Task ConsumeAsync_WhenValid_SendsEmail()
    {
        // Arrange
        var message = new OrderMessage { OrderId = Guid.NewGuid() };
        var order = new OrderModel { Id = message.OrderId, Customer = new CustomerModel { Email = "test@example.com" } };
        var cts = new CancellationTokenSource();

        _mockRepository.Setup(r => r.GetOrderWithDetailsAsync(message.OrderId)).ReturnsAsync(order);
        _mockValidator.Setup(v => v.IsValid(message, order)).Returns(true);
        _mockComposer.Setup(m => m.Map(order)).ReturnsAsync(("Subject", "Body"));

        // Act
        await _consumer.ConsumeAsync(message, cts.Token);

        // Assert
        _mockEmailService.Verify(e => e.SendEmailAsync("Subject", "Body", "test@example.com"), Times.Once);
    }

    [Fact]
    public async Task ConsumeAsync_WhenInvalid_DoesNotSendEmail()
    {
        // Arrange
        var message = new OrderMessage { OrderId = Guid.NewGuid() };
        var order = (OrderModel?)null;
        var cts = new CancellationTokenSource();

        _mockRepository.Setup(r => r.GetOrderWithDetailsAsync(message.OrderId)).ReturnsAsync(order);
        _mockValidator.Setup(v => v.IsValid(message, order)).Returns(false);

        // Act
        await _consumer.ConsumeAsync(message, cts.Token);

        // Assert
        _mockEmailService.Verify(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}
