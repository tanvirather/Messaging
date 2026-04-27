namespace Zuhid.Notification.Messages;

public class VerifyEmailMessage : IMessage
{
    public required string Email { get; set; }
    public required string Token { get; set; }
}
