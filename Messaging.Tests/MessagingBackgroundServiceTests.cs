using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Zuhid.Messaging.Consumers;
using Zuhid.Messaging.Messages;

namespace Zuhid.Messaging.Tests;

public class MessagingBackgroundServiceTests
{
    private readonly Mock<MessagingQueue> _mockQueue = new();
    private readonly Mock<IServiceProvider> _mockServiceProvider = new();
    private readonly Mock<ILogger<MessagingBackgroundService>> _mockLogger = new();
    private readonly MessagingBackgroundService _worker;

    public MessagingBackgroundServiceTests()
    {
        _worker = new MessagingBackgroundService(_mockQueue.Object, _mockServiceProvider.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ExecuteAsync_Dispatches_OrderMessage_To_OrderConsumer()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var message = new OrderMessage { OrderId = orderId };
        var cts = new CancellationTokenSource();

        _mockQueue.Setup(q => q.DequeueMessages(It.IsAny<CancellationToken>()))
            .Returns(TestExtensions.ToAsyncEnumerable([(IMessage)message]));

        var mockOrderConsumer = new Mock<IConsumer<OrderMessage>>();
        mockOrderConsumer.Setup(c => c.ConsumeAsync(It.IsAny<OrderMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var mockScope = new Mock<IServiceScope>();
        var mockScopeServiceProvider = new Mock<IServiceProvider>();

        mockScope.Setup(s => s.ServiceProvider).Returns(mockScopeServiceProvider.Object);
        mockScopeServiceProvider.Setup(s => s.GetService(typeof(OrderConsumer))).Returns(mockOrderConsumer.Object);

        var mockScopeFactory = new Mock<IServiceScopeFactory>();
        _mockServiceProvider.Setup(s => s.GetService(typeof(IServiceScopeFactory))).Returns(mockScopeFactory.Object);
        mockScopeFactory.Setup(s => s.CreateScope()).Returns(mockScope.Object);

        // Act
        var task = _worker.StartAsync(cts.Token);

        await Task.Delay(200);
        cts.Cancel();
        try { await task; } catch (OperationCanceledException) { }

        // Assert
        mockOrderConsumer.Verify(c => c.ConsumeAsync(It.Is<OrderMessage>(m => m.OrderId == orderId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_Dispatches_WelcomeMessage_To_WelcomeConsumer()
    {
        // Arrange
        var message = new WelcomeMessage { Email = "test@example.com", Name = "Test" };
        var cts = new CancellationTokenSource();

        _mockQueue.Setup(q => q.DequeueMessages(It.IsAny<CancellationToken>()))
            .Returns(TestExtensions.ToAsyncEnumerable([(IMessage)message]));

        var mockWelcomeConsumer = new Mock<IConsumer<WelcomeMessage>>();
        mockWelcomeConsumer.Setup(c => c.ConsumeAsync(It.IsAny<WelcomeMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var mockScope = new Mock<IServiceScope>();
        var mockScopeServiceProvider = new Mock<IServiceProvider>();

        mockScope.Setup(s => s.ServiceProvider).Returns(mockScopeServiceProvider.Object);
        mockScopeServiceProvider.Setup(s => s.GetService(typeof(WelcomeConsumer))).Returns(mockWelcomeConsumer.Object);

        var mockScopeFactory = new Mock<IServiceScopeFactory>();
        _mockServiceProvider.Setup(s => s.GetService(typeof(IServiceScopeFactory))).Returns(mockScopeFactory.Object);
        mockScopeFactory.Setup(s => s.CreateScope()).Returns(mockScope.Object);

        // Act
        var task = _worker.StartAsync(cts.Token);

        await Task.Delay(200);
        cts.Cancel();
        try { await task; } catch (OperationCanceledException) { }

        // Assert
        mockWelcomeConsumer.Verify(c => c.ConsumeAsync(It.Is<WelcomeMessage>(m => m.Email == "test@example.com"), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_Dispatches_UnknownMessage_To_GenericConsumer()
    {
        // Arrange
        var message = new UnknownMessage();
        var cts = new CancellationTokenSource();

        _mockQueue.Setup(q => q.DequeueMessages(It.IsAny<CancellationToken>()))
            .Returns(TestExtensions.ToAsyncEnumerable([(IMessage)message]));

        var mockConsumer = new Mock<IConsumer<UnknownMessage>>();
        mockConsumer.Setup(c => c.ConsumeAsync(It.IsAny<UnknownMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var mockScope = new Mock<IServiceScope>();
        var mockScopeServiceProvider = new Mock<IServiceProvider>();

        mockScope.Setup(s => s.ServiceProvider).Returns(mockScopeServiceProvider.Object);
        mockScopeServiceProvider.Setup(s => s.GetService(typeof(IConsumer<UnknownMessage>))).Returns(mockConsumer.Object);

        var mockScopeFactory = new Mock<IServiceScopeFactory>();
        _mockServiceProvider.Setup(s => s.GetService(typeof(IServiceScopeFactory))).Returns(mockScopeFactory.Object);
        mockScopeFactory.Setup(s => s.CreateScope()).Returns(mockScope.Object);

        // Act
        var task = _worker.StartAsync(cts.Token);

        await Task.Delay(200);
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
