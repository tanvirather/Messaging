using Microsoft.AspNetCore.Mvc;
using Zuhid.Notification.Messages;

namespace Zuhid.Notification.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController(NotificationQueue emailQueue) : ControllerBase
{
    [HttpPost("Order")]
    public virtual async Task<IActionResult> Order([FromBody] OrderMessage message) => await QueueMessage(message);

    [HttpPost("Welcome")]
    public virtual async Task<IActionResult> Welcome([FromBody] WelcomeMessage message) => await QueueMessage(message);

    private async Task<IActionResult> QueueMessage(IMessage message)
    {
        await emailQueue.QueueMessage(message);
        return Accepted();
    }
}
