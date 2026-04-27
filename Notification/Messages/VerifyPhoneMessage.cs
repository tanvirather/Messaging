namespace Zuhid.Notification.Messages;

public class VerifyPhoneMessage : IMessage
{
    public required string Phone { get; set; }
    public required string Token { get; set; }
}
