using System.ComponentModel.DataAnnotations;

namespace Zuhid.Notification.Messages;

public class SendTfaTokenMessage : IMessage
{
    [Phone]
    public required string Phone { get; set; }
    [Length(1, 8)]
    public required string Token { get; set; }
}
