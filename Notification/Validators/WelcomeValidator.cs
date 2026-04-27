using Zuhid.Notification.Messages;
using Zuhid.Notification.Models;

namespace Zuhid.Notification.Validators;

public class WelcomeValidator(ILogger<WelcomeValidator> logger)
{
    public virtual bool IsValid(WelcomeMessage message, WelcomeModel? welcome)
    {
        if (welcome == null)
        {
            logger.LogWarning("Customer {CustomerId} not found for welcome email.", message.CustomerId);
            return false;
        }

        if (welcome.Customer == null)
        {
            logger.LogWarning("Customer data for {CustomerId} is missing.", message.CustomerId);
            return false;
        }

        return true;
    }
}
