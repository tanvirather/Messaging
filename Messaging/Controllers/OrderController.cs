using Microsoft.AspNetCore.Mvc;
using Zuhid.Messaging.Mappers;

namespace Zuhid.Messaging.Controllers;

[ApiController]
[Route("Email/[controller]")]

public class OrderController(IEmailService emailService, IOrderMapper orderMapper)
{
    [HttpPost]
    public virtual async Task<bool> Send([FromBody] Guid orderId)
    {
        var (subject, body) = await orderMapper.Map(orderId);
        return await emailService.SendEmailAsync("customer@example.com", subject, body);
    }
}

