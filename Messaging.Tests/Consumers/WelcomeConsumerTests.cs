using Microsoft.Extensions.Logging;
using Moq;
using Zuhid.Messaging.Consumers;
using Zuhid.Messaging.Messages;

namespace Zuhid.Messaging.Tests.Consumers;

public class WelcomeConsumerTests
{
    private readonly Mock<EmailService> _mockEmailService = new(null!);
    private readonly Mock<ILogger<WelcomeConsumer>> _mockLogger = new();
    private readonly WelcomeConsumer _consumer;

    public WelcomeConsumerTests()
    {
        _consumer = new WelcomeConsumer(_mockEmailService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ConsumeAsync_SendsEmail()
    {
        // Arrange
        var message = new WelcomeMessage { Email = "test@example.com", Name = "Test User" };
        var cts = new CancellationTokenSource();

        // Act
        await _consumer.ConsumeAsync(message, cts.Token);

        // Assert
        _mockEmailService.Verify(x => x.SendEmailAsync(
            message.Email,
            It.Is<string>(s => s.Contains(message.Name)),
            It.Is<string>(b => b.Contains(message.Name))),
            Times.Once);
    }

    [Fact]
    public async Task ConsumeAsync_LogsError_OnException()
    {
        // Arrange
        var message = new WelcomeMessage { Email = "test@example.com", Name = "Test User" };
        var cts = new CancellationTokenSource();
        _mockEmailService.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new System.Exception("Send failed"));

        // Act
        await _consumer.ConsumeAsync(message, cts.Token);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error processing welcome email")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
