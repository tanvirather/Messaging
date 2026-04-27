namespace Zuhid.Notification.Messages;

public class OrderMessage : IMessage
{
    public Guid OrderId { get; set; }
}
