using Microsoft.Extensions.Logging;
using Moq;
using Zuhid.Notification.Consumers;
using Zuhid.Notification.Composers;
using Zuhid.Notification.Messages;
using Zuhid.Notification.Models;
using Zuhid.Notification.Repositories;
using Zuhid.Notification.Validators;

namespace Zuhid.Notification.Tests.Consumers;

public class WelcomeConsumerTests
{
    private readonly Mock<EmailService> _mockEmailService;
    private readonly Mock<WelcomeRepository> _mockRepository;
    private readonly Mock<WelcomeComposer> _mockComposer;
    private readonly Mock<WelcomeValidator> _mockValidator;
    private readonly WelcomeConsumer _consumer;

    public WelcomeConsumerTests()
    {
        _mockEmailService = new Mock<EmailService>(null!, null!);
        _mockRepository = new Mock<WelcomeRepository>(null!);
        _mockComposer = new Mock<WelcomeComposer>();
        _mockValidator = new Mock<WelcomeValidator>(new Mock<ILogger<WelcomeValidator>>().Object);

        _consumer = new WelcomeConsumer(
            _mockEmailService.Object,
            _mockRepository.Object,
            _mockComposer.Object,
            _mockValidator.Object);
    }

    [Fact]
    public async Task ConsumeAsync_WhenValid_SendsEmail()
    {
        // Arrange
        var message = new WelcomeMessage { CustomerId = Guid.NewGuid() };
        var welcome = new WelcomeModel
        {
            CustomerId = message.CustomerId,
            Customer = new CustomerModel { Email = "test@example.com" }
        };
        var cts = new CancellationTokenSource();

        _mockRepository.Setup(r => r.GetCustomerWithAddressAsync(message.CustomerId)).ReturnsAsync(welcome);
        _mockValidator.Setup(v => v.IsValid(message, welcome)).Returns(true);
        _mockComposer.Setup(m => m.Map(welcome)).ReturnsAsync(("Welcome", "Body"));

        // Act
        await _consumer.ConsumeAsync(message, cts.Token);

        // Assert
        _mockEmailService.Verify(e => e.SendEmailAsync("Welcome", "Body", "test@example.com"), Times.Once);
    }

    [Fact]
    public async Task ConsumeAsync_WhenInvalid_DoesNotSendEmail()
    {
        // Arrange
        var message = new WelcomeMessage { CustomerId = Guid.NewGuid() };
        var welcome = (WelcomeModel?)null;
        var cts = new CancellationTokenSource();

        _mockRepository.Setup(r => r.GetCustomerWithAddressAsync(message.CustomerId)).ReturnsAsync(welcome);
        _mockValidator.Setup(v => v.IsValid(message, welcome)).Returns(false);

        // Act
        await _consumer.ConsumeAsync(message, cts.Token);

        // Assert
        _mockEmailService.Verify(e => e.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}
