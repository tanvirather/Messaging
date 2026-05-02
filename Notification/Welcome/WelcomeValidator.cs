using Zuhid.Notification.Shared;

namespace Zuhid.Notification.Welcome;

public class WelcomeValidator
{
    public virtual void Validate(WelcomeMessage message, WelcomeModel? welcome)
    {
        if (welcome == null)
        {
            throw new ValidatorException([$"Customer {message.CustomerId} not found for welcome email."]);
        }
    }
}
