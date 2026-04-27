using Zuhid.Notification.Messages;
using Zuhid.Notification.Models;

namespace Zuhid.Notification.Composers;

public class VerifyEmailComposer : BaseComposer
{
    public virtual async Task<(string Subject, string Body)> Map(VerifyEmailMessage model)
    {
        var subject = "Verify Your Email";
        var body = (await ReadTemplate("VerifyEmailComposer.html"))
            .Replace("{email}", model.Email)
            .Replace("{token}", model.Token);
        return (subject, await CreateHtmlAsync(body));
    }
}
