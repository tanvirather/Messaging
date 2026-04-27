using Microsoft.EntityFrameworkCore;
using Zuhid.Notification.Models;

namespace Zuhid.Notification.Repositories;

public class OrderRepository(NotificationContext context)
{
    public virtual async Task<OrderModel?> GetOrderWithDetailsAsync(Guid orderId) => await context.Order
        .Where(o => o.Id == orderId)
        .Select(o => new OrderModel
        {
            Id = o.Id,
            Number = o.Number,
            OrderDate = o.OrderDate,
            TotalAmount = o.TotalAmount,
            Customer = o.Customer != null ? new CustomerModel
            {
                Email = o.Customer.Email
            } : null,
            OrderDetails = o.OrderDetails.Select(od => new OrderDetailModel
            {
                ProductName = od.Product != null ? od.Product.Name : "Unknown Product",
                Quantity = od.Quantity,
                UnitPrice = od.UnitPrice
            }).ToList()
        })
        .FirstOrDefaultAsync();
}
