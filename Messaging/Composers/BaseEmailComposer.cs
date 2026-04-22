using System.Net.Mail;

namespace Zuhid.Messaging.Composers;

public abstract class BaseEmailComposer<TModel> where TModel : class
{
    public virtual MailMessage GetMailMessage(TModel model, string from, string to, string? cc = null, string? bcc = null)
    {
        var mailMessage = new MailMessage
        {
            Subject = GetSubject(model),
            Body = GetBody(model),
            From = new MailAddress(from),
        };
        mailMessage.To.Add(to);
        mailMessage.CC.Add(to);
        mailMessage.Bcc.Add(to);
        return mailMessage;
    }

    public abstract string GetSubject(TModel model);

    public abstract string GetBody(TModel model);
}

