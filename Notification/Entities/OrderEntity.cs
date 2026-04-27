namespace Zuhid.Notification.Entities;

public class OrderEntity
{
    public Guid Id { get; set; }
    public required string Number { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }

    public Guid CustomerId { get; set; }
    public CustomerEntity? Customer { get; set; }

    public ICollection<OrderDetailEntity> OrderDetails { get; set; } = [];
}

