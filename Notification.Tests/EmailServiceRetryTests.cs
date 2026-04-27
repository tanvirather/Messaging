using Moq;
using Zuhid.Notification;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Zuhid.Notification.Tests;

public class EmailServiceRetryTests
{
    private readonly AppSetting _appSetting;

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
    }

    [Fact]
    public async Task SendEmailAsync_RetriesOnFailure()
    {
        // Arrange
        var mockSmtpClient = new Mock<ISmtpClient>();
        mockSmtpClient.Setup(x => x.SendMailAsync(It.IsAny<MailMessage>()))
            .ThrowsAsync(new SmtpException("Transient failure"));

        var service = new EmailService(_appSetting, mockSmtpClient.Object);

        var startTime = DateTime.UtcNow;

        // Act & Assert
        // With 3 retries and 1 min, 2 min delays, it should take at least 3 minutes to fail all attempts.
        // However, we don't want the test to run for 3 minutes.
        // For testing purposes, maybe I should use seconds, but the requirement said minutes.
        // I will change the test to use 0 minutes for one test case or just verify the retry count.

        await Assert.ThrowsAsync<SmtpException>(() =>
            service.SendEmailAsync("Subject", "Body", "to@example.com"));

        mockSmtpClient.Verify(x => x.SendMailAsync(It.IsAny<MailMessage>()), Times.Exactly(3));
    }
}
