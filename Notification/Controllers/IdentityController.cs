using Microsoft.AspNetCore.Mvc;
using Zuhid.Notification.Messages;

namespace Zuhid.Notification.Controllers;

[ApiController]
[Route("[controller]")]
public class IdentityController(NotificationQueue notificationQueue) : ControllerBase
{
    [HttpPost("VerifyEmail")]
    public virtual async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailMessage message) => await QueueMessage(message);

    [HttpPost("VerifyPhone")]
    public virtual async Task<IActionResult> VerifyPhone([FromBody] VerifyPhoneMessage message) => await QueueMessage(message);

    [HttpPost("SendTfaToken")]
    public virtual async Task<IActionResult> SendTfaToken([FromBody] SendTfaTokenMessage message) => await QueueMessage(message);

    private async Task<IActionResult> QueueMessage(IMessage message)
    {
        await notificationQueue.QueueMessage(message);
        return Accepted();
    }
}
