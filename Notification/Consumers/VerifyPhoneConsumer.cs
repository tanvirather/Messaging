using Zuhid.Notification.Composers;
using Zuhid.Notification.Messages;
using Zuhid.Notification.Validators;

namespace Zuhid.Notification.Consumers;

public class VerifyPhoneConsumer(
    EmailService emailService,
    VerifyPhoneComposer composer,
    VerifyPhoneValidator validator) : IConsumer<VerifyPhoneMessage>
{
    public async Task ConsumeAsync(VerifyPhoneMessage message, CancellationToken stoppingToken)
    {
        if (validator.IsValid(message))
        {
            var (subject, body) = await composer.Map(message);
            await emailService.SendEmailAsync(subject, body, "phone@test.com");
        }
    }
}
