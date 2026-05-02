using Zuhid.Notification.Shared;
namespace Zuhid.Notification.Welcome;

public class WelcomeMessage : IMessage
{
    [RequiredGuid]
    public Guid CustomerId { get; set; }
}
