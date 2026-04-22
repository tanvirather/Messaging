using Microsoft.AspNetCore.Mvc;
using Zuhid.Messaging.Mappers;
using Zuhid.Messaging.Repositories;

namespace Zuhid.Messaging.Controllers;

[ApiController]
[Route("Email/[controller]")]
public class OrderController(IEmailService emailService, IOrderMapper orderMapper, OrderRepository orderRepository)
{
    [HttpPost]
    public virtual async Task<bool> Send([FromBody] Guid orderId)
    {
        var order = await orderRepository.GetOrderWithDetailsAsync(orderId);
        if (order == null)
        {
            return false;
        }

        var (subject, body) = await orderMapper.Map(order);
        var recipientEmail = order.Customer?.Email ?? "customer@example.com";

        return await emailService.SendEmailAsync(recipientEmail, subject, body);
    }
}
