using Zuhid.Notification.Messages;

namespace Zuhid.Notification.Consumers;

public interface IConsumer<T> where T : IMessage
{
    Task ConsumeAsync(T message, CancellationToken stoppingToken);
}
