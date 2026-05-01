using System.Threading.Channels;
using Zuhid.Notification.Welcome;

namespace Zuhid.Notification.Shared;

public class NotificationQueue
{
    private readonly Channel<IMessage> _channel = Channel.CreateUnbounded<IMessage>();

    public virtual async ValueTask QueueMessage(IMessage message)
    {
        await _channel.Writer.WriteAsync(message);
    }

    public virtual IAsyncEnumerable<IMessage> DequeueMessages(CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }
}
