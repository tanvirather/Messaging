using Zuhid.Notification.Shared;

namespace Zuhid.Notification.Welcome;

public class WelcomeValidator(ILogger<WelcomeValidator> logger)
{
    public virtual void Validate(WelcomeMessage message, WelcomeModel? welcome)
    {
        if (welcome == null)
        {
            var errorMessage = $"Customer {message.CustomerId} not found for welcome email.";
            logger.LogWarning(errorMessage);
            throw new ValidatorException([errorMessage]);
        }
    }
}
