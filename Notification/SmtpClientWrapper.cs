using System.Net.Mail;

namespace Zuhid.Notification;

public interface ISmtpClient : IDisposable
{
    Task SendMailAsync(MailMessage message);
}

public class SmtpClientWrapper(AppSetting appSetting) : ISmtpClient
{
    private readonly SmtpClient _client = new(appSetting.Smtp.Host, appSetting.Smtp.Port);

    public Task SendMailAsync(MailMessage message) => _client.SendMailAsync(message);

    public void Dispose() => _client.Dispose();
}
