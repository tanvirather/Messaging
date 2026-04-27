namespace Zuhid.Notification.Entities;

public class CustomerEntity
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }

    public ICollection<OrderEntity> Orders { get; set; } = [];
    public ICollection<AddressEntity> Addresses { get; set; } = [];
}
