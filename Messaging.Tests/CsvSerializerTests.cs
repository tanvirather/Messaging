using Xunit;
using Zuhid.Messaging.Models;
using System.IO;

namespace Zuhid.Messaging.Tests;

public class CsvSerializerTests
{
    [Fact]
    public void Load_ShouldReturnListOfObjectsFromCsv()
    {
        // Arrange
        var filePath = "test_customers.csv";
        var csvContent = "Email\ntest1@example.com\ntest2@example.com";
        File.WriteAllText(filePath, csvContent);

        try
        {
            // Act
            var result = CsvSerializer.Load<CustomerModel>(filePath);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("test1@example.com", result[0].Email);
            Assert.Equal("test2@example.com", result[1].Email);
        }
        finally
        {
            // Cleanup
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
    [Fact]
    public void Load_ShouldReturnListOfObjectsWithDifferentTypes()
    {
        // Arrange
        var filePath = "test_orders.csv";
        var csvContent = "Number,OrderDate,TotalAmount\nORD-001,2026-04-21,123.45";
        File.WriteAllText(filePath, csvContent);

        try
        {
            // Act
            var result = CsvSerializer.Load<OrderModel>(filePath);

            // Assert
            Assert.Single(result);
            Assert.Equal("ORD-001", result[0].Number);
            Assert.Equal(new DateTime(2026, 4, 21), result[0].OrderDate);
            Assert.Equal(123.45m, result[0].TotalAmount);
        }
        finally
        {
            // Cleanup
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
