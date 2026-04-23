namespace Zuhid.Messaging.Entities;

public class AddressEntity
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public CustomerEntity? Customer { get; set; }

    public required string Street { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string ZipCode { get; set; }
    public required string Country { get; set; }

    public bool IsDefault { get; set; }
}
