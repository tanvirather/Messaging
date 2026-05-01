using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Zuhid.Notification.Shared;

namespace Zuhid.Notification.Tests.Shared;


public class NotificationBackgroundServiceTests
{
    private readonly Mock<NotificationQueue> _mockQueue = new();
    private readonly Mock<IServiceProvider> _mockServiceProvider = new();
    private readonly Mock<ILogger<NotificationBackgroundService>> _mockLogger = new();
    private readonly NotificationBackgroundService _worker;

    public NotificationBackgroundServiceTests()
    {
        _worker = new NotificationBackgroundService(_mockQueue.Object, _mockServiceProvider.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ExecuteAsync_Dispatches_UnknownMessage_To_GenericConsumer()
    {
        // Arrange
        var message = new UnknownMessage();
        var cts = new CancellationTokenSource();
        var tcs = new TaskCompletionSource();

        _mockQueue.Setup(q => q.DequeueMessages(It.IsAny<CancellationToken>()))
            .Returns(TestExtensions.ToAsyncEnumerable([(IMessage)message]));

        var mockConsumer = new Mock<IConsumer<UnknownMessage>>();
        mockConsumer.Setup(c => c.ConsumeAsync(It.IsAny<UnknownMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Callback(tcs.SetResult);

        var mockScope = new Mock<IServiceScope>();
        var mockScopeServiceProvider = new Mock<IServiceProvider>();

        mockScope.Setup(s => s.ServiceProvider).Returns(mockScopeServiceProvider.Object);
        mockScopeServiceProvider.Setup(s => s.GetService(typeof(IConsumer<UnknownMessage>))).Returns(mockConsumer.Object);

        var mockScopeFactory = new Mock<IServiceScopeFactory>();
        _mockServiceProvider.Setup(s => s.GetService(typeof(IServiceScopeFactory))).Returns(mockScopeFactory.Object);
        mockScopeFactory.Setup(s => s.CreateScope()).Returns(mockScope.Object);

        // Act
        var task = _worker.StartAsync(cts.Token);

        await Task.WhenAny(tcs.Task, Task.Delay(5000));
        cts.Cancel();
        try { await task; } catch (OperationCanceledException) { }

        // Assert
        mockConsumer.Verify(c => c.ConsumeAsync(It.IsAny<UnknownMessage>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}

public class UnknownMessage : IMessage { }

public static class TestExtensions
{
    public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> source)
    {
        foreach (var item in source)
        {
            yield return item;
            await Task.Yield();
        }
    }
}
