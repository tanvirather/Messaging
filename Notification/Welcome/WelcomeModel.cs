namespace Zuhid.Notification.Welcome;

public class WelcomeModel
{
    public Guid CustomerId { get; set; }
    public CustomerModel? Customer { get; set; }
    public AddressModel? DefaultAddress { get; set; }
}
