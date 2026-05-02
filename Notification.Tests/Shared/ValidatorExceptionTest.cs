using Zuhid.Notification.Shared;

namespace Zuhid.Notification.Tests.Shared;

public class ValidatorExceptionTest
{
    [Fact]
    public void Constructor_ShouldJoinMessagesWithSemicolon()
    {
        // Arrange
        var messages = new List<string> { "Error 1", "Error 2", "Error 3" };

        // Act
        var exception = new ValidatorException(messages);

        // Assert
        Assert.Equal("Error 1; Error 2; Error 3", exception.Message);
    }

    [Fact]
    public void Constructor_WhenSingleMessage_ShouldShowMessage()
    {
        // Arrange
        var messages = new List<string> { "Single Error" };

        // Act
        var exception = new ValidatorException(messages);

        // Assert
        Assert.Equal("Single Error", exception.Message);
    }

    [Fact]
    public void Constructor_WhenEmptyList_ShouldHaveEmptyMessage()
    {
        // Arrange
        var messages = new List<string>();

        // Act
        var exception = new ValidatorException(messages);

        // Assert
        Assert.Equal(string.Empty, exception.Message);
    }
}
