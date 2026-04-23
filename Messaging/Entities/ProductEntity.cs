namespace Zuhid.Messaging.Entities;

public class ProductEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }

    public Guid CategoryId { get; set; }
    public CategoryEntity? Category { get; set; }
}
