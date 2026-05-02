using Zuhid.Notification.Shared;

namespace Zuhid.Notification.Tests.Shared;

public class NotificationQueueTest
{
    private class MockMessage : IMessage { }

    [Fact]
    public async Task QueueMessage_And_DequeueMessages_ShouldWorkCorrectly()
    {
        // Arrange
        var queue = new NotificationQueue();
        var message1 = new MockMessage();
        var message2 = new MockMessage();
        var cts = new CancellationTokenSource();

        // Act
        await queue.QueueMessage(message1);
        await queue.QueueMessage(message2);

        var dequeuedMessages = new List<IMessage>();
        var enumerator = queue.DequeueMessages(cts.Token).GetAsyncEnumerator();

        // Read first message
        Assert.True(await enumerator.MoveNextAsync());
        dequeuedMessages.Add(enumerator.Current);

        // Read second message
        Assert.True(await enumerator.MoveNextAsync());
        dequeuedMessages.Add(enumerator.Current);

        // Assert
        Assert.Equal(2, dequeuedMessages.Count);
        Assert.Contains(message1, dequeuedMessages);
        Assert.Contains(message2, dequeuedMessages);

        // Cleanup
        await cts.CancelAsync();
    }
}
