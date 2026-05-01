using System.ComponentModel.DataAnnotations;
using Zuhid.Notification.Shared;

namespace Zuhid.Notification.Tests.Shared;

public class RequiredGuidAttributeTest
{
    [Fact]
    public void IsValid_WhenValueIsNull_ShouldReturnError()
    {
        // Arrange
        var attribute = new RequiredGuidAttribute();
        var context = new ValidationContext(new object()) { MemberName = "TestId" };

        // Act
        var result = attribute.GetValidationResult(null, context);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestId must be a valid Guid.", result.ErrorMessage);
    }

    [Fact]
    public void IsValid_WhenValueIsEmptyGuid_ShouldReturnError()
    {
        // Arrange
        var attribute = new RequiredGuidAttribute();
        var context = new ValidationContext(new object()) { MemberName = "TestId" };

        // Act
        var result = attribute.GetValidationResult(Guid.Empty, context);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestId must be a valid Guid.", result.ErrorMessage);
    }

    [Fact]
    public void IsValid_WhenValueIsNotGuid_ShouldReturnError()
    {
        // Arrange
        var attribute = new RequiredGuidAttribute();
        var context = new ValidationContext(new object()) { MemberName = "TestId" };

        // Act
        var result = attribute.GetValidationResult("not-a-guid", context);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestId must be a valid Guid.", result.ErrorMessage);
    }

    [Fact]
    public void IsValid_WhenValueIsValidGuid_ShouldReturnSuccess()
    {
        // Arrange
        var attribute = new RequiredGuidAttribute();
        var context = new ValidationContext(new object()) { MemberName = "TestId" };

        // Act
        var result = attribute.GetValidationResult(Guid.NewGuid(), context);

        // Assert
        Assert.Equal(ValidationResult.Success, result);
    }
}
