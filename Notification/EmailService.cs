using System.Net.Mail;

namespace Zuhid.Notification;

public class EmailService(AppSetting appSetting, ISmtpClient smtpClient, ILogger<EmailService> logger)
{
    public virtual async Task SendEmailAsync(string subject, string body, string to, string? cc = null, string? bcc = null)
    {
        var retryCount = appSetting.Smtp.RetryCount;
        var delay = appSetting.Smtp.RetryInterval;

        for (var i = 0; i < retryCount; i++)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(appSetting.Smtp.From),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(to);

                if (!string.IsNullOrWhiteSpace(cc))
                {
                    mailMessage.CC.Add(cc);
                }

                if (!string.IsNullOrWhiteSpace(bcc))
                {
                    mailMessage.Bcc.Add(bcc);
                }

                await smtpClient.SendMailAsync(mailMessage);
                return;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"{subject}: {i}");

                if (i == retryCount - 1)
                {
                    throw;
                }
                await Task.Delay(delay);
                delay *= 2;
            }
        }
    }

    public virtual async Task SendTextAsync(string phone, string body)
    {
        var retryCount = appSetting.Smtp.RetryCount;
        var delay = appSetting.Smtp.RetryInterval;

        for (var i = 0; i < retryCount; i++)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(appSetting.Smtp.From),
                    Subject = $"{phone}",
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add("phone@test.com");
                await smtpClient.SendMailAsync(mailMessage);
                return;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, $"{phone}: {i}");

                if (i == retryCount - 1)
                {
                    throw;
                }
                await Task.Delay(delay);
                delay *= 2;
            }
        }
    }

}
