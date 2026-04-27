using Zuhid.Notification.Composers;
using Zuhid.Notification.Messages;

namespace Zuhid.Notification.Consumers;

public class SendTfaTokenConsumer(EmailService emailService, SendTfaTokenComposer composer) : IConsumer<SendTfaTokenMessage>
{
    public async Task ConsumeAsync(SendTfaTokenMessage message, CancellationToken stoppingToken)
    {
        var body = await composer.Map(message);
        await emailService.SendTextAsync(message.Phone, body);
    }
}

