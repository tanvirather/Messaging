using System.Text;
using Zuhid.Notification.Models;

namespace Zuhid.Notification.Composers;

public class OrderComposer : BaseComposer
{
    public virtual async Task<(string Subject, string Body)> Map(OrderModel order)
    {
        var subject = $"Order Confirmation - {order.Number}";

        var detailsBuilder = new StringBuilder();
        foreach (var detail in order.OrderDetails)
        {
            detailsBuilder.Append($"""
                <tr>
                    <td>{detail.ProductName}</td>
                    <td>{detail.Quantity}</td>
                    <td>{detail.UnitPrice:C}</td>
                    <td>{detail.Quantity * detail.UnitPrice:C}</td>
                </tr>
            """);
        }

        var body = (await ReadTemplate("OrderComposer.html"))
            .Replace("{orderId}", order.Id.ToString())
            .Replace("{orderNumber}", order.Number)
            .Replace("{orderDetailsTable}", detailsBuilder.ToString())
            .Replace("{totalAmount}", order.TotalAmount.ToString("C"));

        return (subject, await CreateHtmlAsync(body));
    }
}
