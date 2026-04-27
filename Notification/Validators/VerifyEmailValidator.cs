using Zuhid.Notification.Messages;

namespace Zuhid.Notification.Validators;

public class VerifyEmailValidator(ILogger<VerifyEmailValidator> logger)
{
    public virtual bool IsValid(VerifyEmailMessage message)
    {
        if (message == null)
        {
            logger.LogWarning("VerifyEmailMessage is null");
            return false;
        }

        if (string.IsNullOrEmpty(message.Email))
        {
            logger.LogWarning("message.Email is not valid.");
            return false;
        }
        return true;
    }
}
