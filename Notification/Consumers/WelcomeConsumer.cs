using Zuhid.Notification.Composers;
using Zuhid.Notification.Messages;
using Zuhid.Notification.Repositories;
using Zuhid.Notification.Validators;

namespace Zuhid.Notification.Consumers;

public class WelcomeConsumer(
    EmailService emailService,
    WelcomeRepository repository,
    WelcomeComposer composer,
    WelcomeValidator validator) : IConsumer<WelcomeMessage>
{
    public async Task ConsumeAsync(WelcomeMessage message, CancellationToken stoppingToken)
    {
        var welcome = await repository.GetCustomerWithAddressAsync(message.CustomerId);
        if (validator.IsValid(message, welcome))
        {
            var (subject, body) = await composer.Map(welcome!);
            await emailService.SendEmailAsync(subject, body, welcome!.Customer!.Email);
        }
    }
}
