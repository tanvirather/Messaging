using Zuhid.Messaging.Repositories;

namespace Zuhid.Messaging.Tests.Repositories;

[Collection("Database collection")]
public class OrderRepositoryTests(TestDatabaseFixture fixture)
{
    private readonly TestDatabaseFixture _fixture = fixture;

    [Fact]
    public async Task GetOrderWithDetailsAsync_Returns_Order_When_Exists()
    {
        // Arrange
        var context = _fixture.Context;
        var repository = new OrderRepository(context);

        // Get an ID from the seeded data (OrderEntity.csv)
        var orderId = Guid.Parse("a149b5b1-425f-43d6-a9ed-5f51433b23a5");

        // Act
        var result = await repository.GetOrderWithDetailsAsync(orderId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderId, result.Id);
        Assert.Equal("ORD-002", result.Number);
        Assert.Equal(DateTimeKind.Utc, result.OrderDate.Kind);
    }

    [Fact]
    public async Task GetOrderWithDetailsAsync_Returns_Null_When_Not_Exists()
    {
        // Arrange
        var context = _fixture.Context;
        var repository = new OrderRepository(context);
        var orderId = Guid.NewGuid();

        // Act
        var result = await repository.GetOrderWithDetailsAsync(orderId);

        // Assert
        Assert.Null(result);
    }
}
