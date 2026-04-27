namespace Zuhid.Notification.Messages;

public class WelcomeMessage : IMessage
{
    public Guid CustomerId { get; set; }
}
