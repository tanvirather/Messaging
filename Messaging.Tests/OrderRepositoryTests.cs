using Microsoft.EntityFrameworkCore;
using Zuhid.Messaging.Entities;
using Zuhid.Messaging.Repositories;
using Zuhid.Messaging.Models;
using Xunit;

namespace Zuhid.Messaging.Tests;

[Collection("Database collection")]
public class OrderRepositoryTests
{
    private readonly TestDatabaseFixture _fixture;

    public OrderRepositoryTests(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }

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
    }

    // [Fact]
    // public async Task GetOrderWithDetailsAsync_Handles_Null_Customer_And_Product()
    // {
    //     // Arrange
    //     var context = _fixture.Context;
    //     var repository = new OrderRepository(context);

    //     var orderId = Guid.NewGuid();
    //     var order = new OrderEntity
    //     {
    //         Id = orderId,
    //         Number = "ORD-002",
    //         OrderDate = DateTime.UtcNow,
    //         TotalAmount = 50,
    //         Customer = null
    //     };

    //     var detail = new OrderDetailEntity
    //     {
    //         Id = Guid.NewGuid(),
    //         OrderId = orderId,
    //         Product = null,
    //         Quantity = 1,
    //         UnitPrice = 50
    //     };

    //     context.Order.Add(order);
    //     context.OrderDetail.Add(detail);
    //     await context.SaveChangesAsync();

    //     // Act
    //     var result = await repository.GetOrderWithDetailsAsync(orderId);

    //     // Assert
    //     Assert.NotNull(result);
    //     Assert.Null(result.Customer);
    //     Assert.Single(result.OrderDetails);
    //     Assert.Equal("Unknown Product", result.OrderDetails[0].ProductName);
    // }

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
