using Zuhid.Notification.Shared;

namespace Zuhid.Notification.Welcome;

public class WelcomeConsumer(
    EmailService emailService,
    WelcomeRepository repository,
    WelcomeMapper mapper,
    WelcomeValidator validator) : IConsumer<WelcomeMessage>
{
    public async Task ConsumeAsync(WelcomeMessage message, CancellationToken stoppingToken)
    {
        var welcome = await repository.Get(message.CustomerId);
        validator.Validate(message, welcome);
        var mailMessage = await mapper.Map(welcome!);
        mailMessage.To.Add("test@customer.com");
        await emailService.SendEmailAsync(mailMessage);
    }
}
