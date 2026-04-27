using Moq;
using Zuhid.Notification.Messages;

namespace Zuhid.Notification.Tests;

public class NotificationQueueTests
{
    [Fact]
    public async Task QueueAndDequeue_WorksCorrectly()
    {
        // Arrange
        var queue = new NotificationQueue();
        var message = new Mock<IMessage>().Object;
        var cts = new CancellationTokenSource();

        // Act
        await queue.QueueMessage(message);

        var iterator = queue.DequeueMessages(cts.Token).GetAsyncEnumerator();
        var hasNext = await iterator.MoveNextAsync();

        // Assert
        Assert.True(hasNext);
        Assert.Same(message, iterator.Current);
    }
}
