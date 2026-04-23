using Zuhid.Messaging.Messages;

namespace Zuhid.Messaging.Consumers;

public interface IConsumer<T> where T : IMessage
{
    Task ConsumeAsync(T message, CancellationToken stoppingToken);
}
