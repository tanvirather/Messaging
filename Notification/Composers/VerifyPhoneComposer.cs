using Zuhid.Notification.Messages;

namespace Zuhid.Notification.Composers;

public class VerifyPhoneComposer : BaseComposer
{
    public virtual async Task<(string Subject, string Body)> Map(VerifyPhoneMessage model)
    {
        var subject = "Verify Your Phone Number";
        var body = (await ReadTemplate("VerifyPhoneComposer.html"))
            .Replace("{phone}", model.Phone)
            .Replace("{token}", model.Token);
        return (subject, await CreateHtmlAsync(body));
    }
}
