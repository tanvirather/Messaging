using System.Net.Mail;

namespace Zuhid.Messaging;

public class EmailService(AppSetting appSetting)
{
    public virtual async Task SendEmailAsync(string to, string subject, string body)
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
        await client.SendMailAsync(mailMessage);
    }
}
