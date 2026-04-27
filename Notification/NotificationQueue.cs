using System.Threading.Channels;
using Zuhid.Notification.Messages;

namespace Zuhid.Notification;

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
