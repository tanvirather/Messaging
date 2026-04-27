using Zuhid.Notification.Messages;

namespace Zuhid.Notification.Composers;

public class SendTfaTokenComposer : BaseComposer
{
    public virtual async Task<string> Map(SendTfaTokenMessage model)
    {
        var body = (await ReadTemplate("SendTfaTokenComposer.html"))
            .Replace("{phone}", model.Phone)
            .Replace("{token}", model.Token);
        return await CreateHtmlAsync(body);
    }
}
