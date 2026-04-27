namespace Zuhid.Notification.Models;

public class WelcomeModel
{
    public Guid CustomerId { get; set; }
    public CustomerModel? Customer { get; set; }
    public AddressModel? DefaultAddress { get; set; }
}
