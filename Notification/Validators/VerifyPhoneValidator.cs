using Zuhid.Notification.Messages;

namespace Zuhid.Notification.Validators;

public class VerifyPhoneValidator(ILogger<VerifyPhoneValidator> logger)
{
    public virtual bool IsValid(VerifyPhoneMessage message)
    {
        if (message == null)
        {
            logger.LogWarning("VerifyEmailMessage is null");
            return false;
        }

        if (string.IsNullOrEmpty(message.Phone))
        {
            logger.LogWarning("Phone is not valid.");
            return false;
        }
        return true;
    }
}
