namespace Zuhid.Messaging.Messages;

public class WelcomeMessage : IMessage
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
