namespace Zuhid.Messaging.Entities;

public class CustomerEntity
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }

    public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
    public ICollection<AddressEntity> Addresses { get; set; } = new List<AddressEntity>();
}
