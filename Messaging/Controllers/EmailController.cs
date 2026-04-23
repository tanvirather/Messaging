using Microsoft.AspNetCore.Mvc;
using Zuhid.Messaging.Messages;

namespace Zuhid.Messaging.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController(MessagingQueue emailQueue) : ControllerBase
{
    [HttpPost("Order")] // a0b1c2d3-e4f5-a6b7-c8d9-e0f1a2b3c4d5
    public virtual async Task<IActionResult> Order([FromBody] OrderMessage message) => await QueueMessage(message);

    private async Task<IActionResult> QueueMessage(IMessage message)
    {
        await emailQueue.QueueMessage(message);
        return Accepted();
    }
}
