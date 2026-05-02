using Microsoft.AspNetCore.Mvc;
using Zuhid.Notification.Shared;
using Zuhid.Notification.Welcome;

namespace Zuhid.Notification.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController(NotificationQueue emailQueue) : ControllerBase
{
    [HttpPost("Welcome")]
    public async Task Welcome([FromBody] WelcomeMessage message) => await emailQueue.QueueMessage(message);

}
