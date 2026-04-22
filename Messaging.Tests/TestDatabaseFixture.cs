using Microsoft.EntityFrameworkCore;
using Zuhid.Messaging;
using Zuhid.Messaging.Entities;
using System.Globalization;
using System.Reflection;

namespace Zuhid.Messaging.Tests;

public class TestDatabaseFixture : IDisposable
{
    public MessagingContext Context { get; private set; }

    public TestDatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<MessagingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new MessagingContext(options);
        SeedFromCsv();
    }

    private void SeedFromCsv()
    {
        Context.Category.LoadFromCsv("CategoryEntity.csv");
        Context.Customer.LoadFromCsv("CustomerEntity.csv");
        Context.Product.LoadFromCsv("ProductEntity.csv");
        Context.Order.LoadFromCsv("OrderEntity.csv");
        Context.OrderDetail.LoadFromCsv("OrderDetailEntity.csv");

        Context.SaveChanges();
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}
