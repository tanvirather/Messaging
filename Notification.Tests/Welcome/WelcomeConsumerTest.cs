using Moq;
using System.Net.Mail;
using Zuhid.Notification.Shared;
using Zuhid.Notification.Welcome;

namespace Zuhid.Notification.Tests.Welcome;

public class WelcomeConsumerTest
{
    private readonly Mock<EmailService> _emailServiceMock;
    private readonly Mock<WelcomeRepository> _repositoryMock;
    private readonly Mock<WelcomeMapper> _mapperMock;
    private readonly Mock<WelcomeValidator> _validatorMock;
    private readonly WelcomeConsumer _consumer;

    public WelcomeConsumerTest()
    {
        _emailServiceMock = new Mock<EmailService>(null!, null!, null!);
        _repositoryMock = new Mock<WelcomeRepository>(null!);
        _mapperMock = new Mock<WelcomeMapper>();
        _validatorMock = new Mock<WelcomeValidator>(null!);

        _consumer = new WelcomeConsumer(
            _emailServiceMock.Object,
            _repositoryMock.Object,
            _mapperMock.Object,
            _validatorMock.Object);
    }

    [Fact]
    public async Task ConsumeAsync_WhenValid_ShouldSendEmail()
    {
        // Arrange
        var message = new WelcomeMessage { CustomerId = Guid.NewGuid() };
        var welcomeModel = new WelcomeModel();
        var mailMessage = new MailMessage();
        var stoppingToken = CancellationToken.None;

        _repositoryMock.Setup(x => x.Get(message.CustomerId)).ReturnsAsync(welcomeModel);
        _mapperMock.Setup(x => x.Map(welcomeModel)).ReturnsAsync(mailMessage);

        // Act
        await _consumer.ConsumeAsync(message, stoppingToken);

        // Assert
        _validatorMock.Verify(x => x.Validate(message, welcomeModel), Times.Once);
        _emailServiceMock.Verify(x => x.SendEmailAsync(It.Is<MailMessage>(m => m.To.Any(t => t.Address == "test@customer.com"))), Times.Once);
    }

    [Fact]
    public async Task ConsumeAsync_WhenValidatorThrows_ShouldNotSendEmail()
    {
        // Arrange
        var message = new WelcomeMessage { CustomerId = Guid.NewGuid() };
        var welcomeModel = (WelcomeModel?)null;
        var stoppingToken = CancellationToken.None;

        _repositoryMock.Setup(x => x.Get(message.CustomerId)).ReturnsAsync(welcomeModel);
        _validatorMock.Setup(x => x.Validate(message, welcomeModel)).Throws(new ValidatorException(["Error"]));

        // Act & Assert
        await Assert.ThrowsAsync<ValidatorException>(() => _consumer.ConsumeAsync(message, stoppingToken));

        _emailServiceMock.Verify(x => x.SendEmailAsync(It.IsAny<MailMessage>()), Times.Never);
    }
}
