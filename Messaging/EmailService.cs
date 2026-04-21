using System.Net.Mail;
using System.Net.Sockets;
// using Microsoft.Extensions.Logging;

namespace Zuhid.Messaging;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string body);
}

public class EmailService(AppSetting appSetting, ILogger<EmailService> logger) : IEmailService
{
    public async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient(appSetting.Smtp.Host, appSetting.Smtp.Port);
        var mailMessage = new MailMessage
        {
            From = new MailAddress(appSetting.Smtp.From),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };
        mailMessage.To.Add(to);

        try
        {
            await client.SendMailAsync(mailMessage);
            return true;
        }
        catch (SmtpException ex) when (ex.InnerException is SocketException { SocketErrorCode: SocketError.ConnectionRefused })
        {
            logger.LogError(ex, "SMTP connection refused. Ensure MailHog is running at {Host}:{Port}.", appSetting.Smtp.Host, appSetting.Smtp.Port);
            throw new Exception($"Failed to send email: The SMTP server at {appSetting.Smtp.Host}:{appSetting.Smtp.Port} is not reachable. Please ensure MailHog or a similar SMTP server is running.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while sending email.");
            throw;
        }
    }
}
