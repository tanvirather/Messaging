namespace Zuhid.Notification.Models;

public class OrderModel
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public CustomerModel? Customer { get; set; }
    public List<OrderDetailModel> OrderDetails { get; set; } = [];
}
