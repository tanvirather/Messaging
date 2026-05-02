using Microsoft.EntityFrameworkCore;
using Zuhid.Notification.Shared;

namespace Zuhid.Notification.Welcome;

public class WelcomeRepository(NotificationContext context)
{
    public virtual async Task<WelcomeModel?> Get(Guid customerId) => await context.Customer
        .Where(c => c.Id == customerId)
        .Select(c => new WelcomeModel
        {
            CustomerId = c.Id,
            Customer = new CustomerModel
            {
                Email = c.Email,
                FirstName = c.FirstName,
                LastName = c.LastName,
                PhoneNumber = c.PhoneNumber
            },
            DefaultAddress = c.Addresses
                .Where(a => a.IsDefault)
                .Select(a => new AddressModel
                {
                    Street = a.Street,
                    City = a.City,
                    State = a.State,
                    ZipCode = a.ZipCode,
                    Country = a.Country
                })
                .FirstOrDefault()
        })
        .FirstOrDefaultAsync();
}
