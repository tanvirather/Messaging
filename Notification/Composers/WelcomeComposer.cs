using Zuhid.Notification.Models;

namespace Zuhid.Notification.Composers;

public class WelcomeComposer : BaseComposer
{
    public virtual async Task<(string Subject, string Body)> Map(WelcomeModel welcome)
    {
        var subject = "Welcome to Our Platform!";
        var customer = welcome.Customer!;
        var address = welcome.DefaultAddress ?? new AddressModel();

        var body = (await ReadTemplate("WelcomeComposer.html"))
            .Replace("{firstName}", customer.FirstName ?? string.Empty)
            .Replace("{lastName}", customer.LastName ?? string.Empty)
            .Replace("{email}", customer.Email)
            .Replace("{phoneNumber}", customer.PhoneNumber ?? "Not provided")
            .Replace("{street}", address.Street)
            .Replace("{city}", address.City)
            .Replace("{state}", address.State)
            .Replace("{zipCode}", address.ZipCode)
            .Replace("{country}", address.Country);

        return (subject, await CreateHtmlAsync(body));
    }
}
