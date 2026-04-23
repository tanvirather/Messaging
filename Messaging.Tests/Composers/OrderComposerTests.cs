using Zuhid.Messaging.Composers;
using Zuhid.Messaging.Models;

namespace Zuhid.Messaging.Tests.Composers;

public class OrderComposerTests
{
    private readonly OrderComposer _composer = new();

    [Fact]
    public async Task Map_Returns_Correct_Subject_And_Body()
    {
        // Arrange
        var order = new OrderModel
        {
            Id = Guid.NewGuid(),
            Number = "ORD-123",
            TotalAmount = 150.50m,
            OrderDetails =
            [
                new OrderDetailModel
                {
                    ProductName = "Product A",
                    Quantity = 2,
                    UnitPrice = 50.25m
                },
                new OrderDetailModel
                {
                    ProductName = "Product B",
                    Quantity = 1,
                    UnitPrice = 50.00m
                }
            ]
        };

        // Act
        var (subject, body) = await _composer.Map(order);

        // Assert
        Assert.Equal($"Order Confirmation - {order.Number}", subject);
        Assert.Contains(order.Id.ToString(), body);
        Assert.Contains(order.Number, body);
        Assert.Contains("Product A", body);
        Assert.Contains("Product B", body);
        Assert.Contains("150.50", body); // Total amount check (might be formatted with currency)
        Assert.Contains("<html>", body);
    }

    [Fact]
    public async Task Map_Handles_Empty_Details()
    {
        // Arrange
        var order = new OrderModel
        {
            Id = Guid.NewGuid(),
            Number = "ORD-EMPTY",
            TotalAmount = 0,
            OrderDetails = []
        };

        // Act
        var (subject, body) = await _composer.Map(order);

        // Assert
        Assert.Equal($"Order Confirmation - {order.Number}", subject);
        Assert.Contains("ORD-EMPTY", body);
    }
}
