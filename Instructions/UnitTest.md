# Unit Testing Guidelines for New Email Notifications

This document outlines how to write unit tests for new email notification types, following the established patterns in the `Notification.Tests` project.

## 1. Composer Tests
Create a test class in `Notification.Tests/Composers/`. Test the `Map` method to ensure placeholders are correctly replaced and the subject is as expected.

**Example: `WelcomeComposerTests.cs`**
```csharp
public class WelcomeComposerTests
{
    private readonly WelcomeComposer _composer = new();

    [Fact]
    public async Task Map_Returns_Correct_Subject_And_Body()
    {
        var model = new WelcomeModel { ... };
        var (subject, body) = await _composer.Map(model);
        Assert.Equal("Expected Subject", subject);
        Assert.Contains("Expected Content", body);
    }
}
```

## 2. Validator Tests
Create a test class in `Notification.Tests/Validators/`. Test the `IsValid` method with both valid and invalid scenarios (e.g., null models, missing data).

**Example: `WelcomeValidatorTests.cs`**
```csharp
public class WelcomeValidatorTests
{
    private readonly WelcomeValidator _validator = new(new Mock<ILogger<WelcomeValidator>>().Object);

    [Fact]
    public void IsValid_WhenValid_ReturnsTrue() { ... }

    [Fact]
    public void IsValid_WhenNull_ReturnsFalse() { ... }
}
```

## 3. Consumer Tests
Create a test class in `Notification.Tests/Consumers/`. Use `Moq` to mock dependencies (`EmailService`, `Repository`, `Composer`, `Validator`). Verify that `SendEmailAsync` is called when data is valid and NOT called when invalid.

**Example: `WelcomeConsumerTests.cs`**
```csharp
public class WelcomeConsumerTests
{
    // Mock dependencies
    // Setup mocks
    // Act: await _consumer.ConsumeAsync(message, token);
    // Assert: _mockEmailService.Verify(..., Times.Once);
}
```

## 4. Controller Tests
Create or update tests in `Notification.Tests/Controllers/`. Test that API endpoints correctly queue messages.

**Example: `EmailControllerTests.cs`**
```csharp
public class EmailControllerTests
{
    private readonly Mock<NotificationQueue> _mockQueue = new();
    private readonly EmailController _controller;

    public EmailControllerTests()
    {
        _controller = new EmailController(_mockQueue.Object);
    }

    [Fact]
    public async Task Welcome_Returns_Accepted_And_Queues_Message()
    {
        var message = new WelcomeMessage { CustomerId = Guid.NewGuid() };
        var result = await _controller.Welcome(message);
        Assert.IsType<AcceptedResult>(result);
        _mockQueue.Verify(x => x.QueueMessage(It.IsAny<WelcomeMessage>()), Times.Once);
    }
}
```

## 5. Repository Tests (Optional/Integration)
If you need to test database projection, create tests in `Notification.Tests/Repositories/`. These usually require an In-Memory database or a SQLite test database.

## Running Tests
Run all tests in the solution:
```powershell
dotnet test
```
Or run specific tests:
```powershell
dotnet test --filter FullyQualifiedName~Welcome
```
