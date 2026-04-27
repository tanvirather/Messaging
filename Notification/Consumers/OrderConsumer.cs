using Zuhid.Notification.Composers;
using Zuhid.Notification.Messages;
using Zuhid.Notification.Repositories;
using Zuhid.Notification.Validators;

namespace Zuhid.Notification.Consumers;

public class OrderConsumer(EmailService emailService, OrderRepository repository, OrderComposer composer, OrderValidator validator) : IConsumer<OrderMessage>
{
    public async Task ConsumeAsync(OrderMessage message, CancellationToken stoppingToken)
    {
        // Thread.Sleep(3000);
        var order = await repository.GetOrderWithDetailsAsync(message.OrderId);
        if (validator.IsValid(message, order))
        {
            var (subject, body) = await composer.Map(order!);
            await emailService.SendEmailAsync(subject, body, order!.Customer!.Email);
        }
    }
}
