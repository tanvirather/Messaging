namespace Zuhid.Messaging.Messages;

public class OrderMessage : IMessage
{
    public Guid OrderId { get; set; }
}
