using Moq;
using Zuhid.Messaging.Messages;

namespace Zuhid.Messaging.Tests;

public class MessagingQueueTests
{
    [Fact]
    public async Task QueueAndDequeue_WorksCorrectly()
    {
        // Arrange
        var queue = new MessagingQueue();
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
