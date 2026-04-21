namespace Zuhid.Messaging.Entities;

public class OrderDetailEntity
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public required string ItemNumber { get; set; }
    public required string Description { get; set; }
    public required decimal Price { get; set; }
}

