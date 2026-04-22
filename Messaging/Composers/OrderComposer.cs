using Zuhid.Messaging.Models;

namespace Zuhid.Messaging.Composers;

public class OrderComposer : BaseEmailComposer<OrderModel>
{
    public override string GetSubject(OrderModel model)
    {
        return "subject";
    }

    public override string GetBody(OrderModel model)
    {
        return "body";
    }
}
