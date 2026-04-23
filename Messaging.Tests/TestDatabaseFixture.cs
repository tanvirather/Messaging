using Microsoft.EntityFrameworkCore;
using Zuhid.Base;

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
        Context.Category.LoadCsvData("Dataload/CategoryEntity.csv");
        Context.Customer.LoadCsvData("Dataload/CustomerEntity.csv");
        Context.Product.LoadCsvData("Dataload/ProductEntity.csv");
        Context.Order.LoadCsvData("Dataload/OrderEntity.csv");
        Context.OrderDetail.LoadCsvData("Dataload/OrderDetailEntity.csv");

        Context.SaveChanges();
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<TestDatabaseFixture>
{
}
