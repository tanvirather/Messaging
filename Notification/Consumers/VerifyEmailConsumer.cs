using Zuhid.Notification.Composers;
using Zuhid.Notification.Messages;
using Zuhid.Notification.Models;
using Zuhid.Notification.Validators;

namespace Zuhid.Notification.Consumers;

public class VerifyEmailConsumer(
    EmailService emailService,
    VerifyEmailComposer composer,
    VerifyEmailValidator validator) : IConsumer<VerifyEmailMessage>
{
    public async Task ConsumeAsync(VerifyEmailMessage message, CancellationToken stoppingToken)
    {
        if (validator.IsValid(message))
        {
            var (subject, body) = await composer.Map(message);
            await emailService.SendEmailAsync(subject, body, message.Email);
        }
    }
}
