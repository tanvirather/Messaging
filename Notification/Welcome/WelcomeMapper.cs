using System.Net.Mail;
using Zuhid.Notification.Shared;

namespace Zuhid.Notification.Welcome;

public class WelcomeMapper() : BaseMapper("Welcome")
{
    public virtual async Task<MailMessage> Map(WelcomeModel welcome)
    {
        var subject = "Welcome to Our Platform!";
        var customer = welcome.Customer!;
        var address = welcome.DefaultAddress ?? new AddressModel();
        var body = (await ReadTemplate("Welcome.html"))
            .Replace("{{firstName}}", customer.FirstName ?? string.Empty)
            .Replace("{{lastName}}", customer.LastName ?? string.Empty)
            .Replace("{{email}}", customer.Email)
            .Replace("{{phoneNumber}}", customer.PhoneNumber ?? "Not provided")
            .Replace("{{street}}", address.Street)
            .Replace("{{city}}", address.City)
            .Replace("{{state}}", address.State)
            .Replace("{{zipCode}}", address.ZipCode)
            .Replace("{{country}}", address.Country);
        return new MailMessage
        {
            Subject = subject,
            Body = await CreateHtmlAsync(body),
            IsBodyHtml = true
        };
    }
}
