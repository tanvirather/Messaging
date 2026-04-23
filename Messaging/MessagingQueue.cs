using System.Threading.Channels;
using Zuhid.Messaging.Messages;

namespace Zuhid.Messaging;

public class MessagingQueue
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
