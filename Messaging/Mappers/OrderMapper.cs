namespace Zuhid.Messaging.Mappers;

public interface IOrderMapper
{
    Task<(string Subject, string Body)> Map(Guid orderId);
}

public class OrderMapper : EmailMapper, IOrderMapper
{
    public async Task<(string Subject, string Body)> Map(Guid orderId)
    {
        var subject = $"Order Confirmation - {orderId}";
        var body = (await ReadTemplate("OrderConfirmation.html"))
            .Replace("{orderId}", orderId.ToString());
        return (subject, await CreateHtmlAsync(body));
    }
}
