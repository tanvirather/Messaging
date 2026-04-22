namespace Zuhid.Messaging.Entities;

public class CategoryEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    public ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}
