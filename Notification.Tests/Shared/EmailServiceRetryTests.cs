using Moq;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Zuhid.Notification.Shared.Tests;

public class EmailServiceRetryTests
{
    private readonly AppSetting _appSetting;
    private readonly Mock<ILogger<EmailService>> _mockLogger;

    public EmailServiceRetryTests()
    {
        var myConfiguration = new Dictionary<string, string?>
        {
            {"AppSettings:Smtp:Host", "localhost"},
            {"AppSettings:Smtp:Port", "25"},
            {"AppSettings:Smtp:From", "sender@example.com"},
            {"AppSettings:Smtp:RetryCount", "3"},
            {"AppSettings:Smtp:RetryInterval", "00:00:00"},
            {"ConnectionStrings:Notification", "Host=localhost;Username=[postgres_credential];Password=password"},
            {"ConnectionStrings:Log", "Host=localhost;Username=[postgres_credential];Password=password"},
            {"postgres_credential", "dummy"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(myConfiguration)
            .Build();

        _appSetting = new AppSetting(configuration);
        _mockLogger = new Mock<ILogger<EmailService>>();
    }

    [Fact]
    public async Task SendEmailAsync_RetriesOnFailure()
    {
        // Arrange
        var mockSmtpClient = new Mock<ISmtpClient>();
        mockSmtpClient.Setup(x => x.SendMailAsync(It.IsAny<MailMessage>()))
            .ThrowsAsync(new SmtpException("Transient failure"));

        var service = new EmailService(_appSetting, mockSmtpClient.Object, _mockLogger.Object);

        // Act & Assert
        await Assert.ThrowsAsync<SmtpException>(() =>
            service.SendEmailAsync("Subject", "Body", "to@example.com"));

        mockSmtpClient.Verify(x => x.SendMailAsync(It.IsAny<MailMessage>()), Times.Exactly(3));
    }

    [Fact]
    public async Task SendTextAsync_RetriesOnFailure()
    {
        // Arrange
        var mockSmtpClient = new Mock<ISmtpClient>();
        mockSmtpClient.Setup(x => x.SendMailAsync(It.IsAny<MailMessage>()))
            .ThrowsAsync(new SmtpException("Transient failure"));

        var service = new EmailService(_appSetting, mockSmtpClient.Object, _mockLogger.Object);

        // Act & Assert
        await Assert.ThrowsAsync<SmtpException>(() =>
            service.SendTextAsync("1234567890", "Body"));

        mockSmtpClient.Verify(x => x.SendMailAsync(It.IsAny<MailMessage>()), Times.Exactly(3));
    }
}
