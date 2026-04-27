using Zuhid.Notification.Messages;

namespace Zuhid.Notification.Validators;

public class OrderValidator(ILogger<OrderValidator> logger)
{
    public virtual bool IsValid(OrderMessage message, Models.OrderModel? order)
    {
        var isValid = true;
        if (order == null)
        {
            logger.LogWarning("Order {OrderId} not found.", message.OrderId);
            isValid = false;
        }
        else if (order.Customer == null)
        {
            logger.LogWarning("Customer for {OrderId} not found.", message.OrderId);
            isValid = false;
        }
        return isValid;
    }
}
