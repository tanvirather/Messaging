using Zuhid.Messaging.Composers;
using Zuhid.Messaging.Messages;
using Zuhid.Messaging.Repositories;
using Zuhid.Messaging.Validators;

namespace Zuhid.Messaging.Consumers;

public class OrderConsumer(EmailService emailService, OrderRepository repository, OrderComposer composer, OrderValidator validator) : IConsumer<OrderMessage>
{
    public async Task ConsumeAsync(OrderMessage message, CancellationToken stoppingToken)
    {
        // Thread.Sleep(3000);
        var order = await repository.GetOrderWithDetailsAsync(message.OrderId);
        if (validator.IsValid(message, order))
        {
            var (subject, body) = await composer.Map(order!);
            await emailService.SendEmailAsync(order!.Customer!.Email, subject, body);
        }
    }
}
