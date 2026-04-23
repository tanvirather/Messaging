using System.Text.Json;
using Zuhid.Messaging.Consumers;
using Zuhid.Messaging.Messages;

namespace Zuhid.Messaging;

public class MessagingBackgroundService(MessagingQueue queue, IServiceProvider serviceProvider, ILogger<MessagingBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Messaging background worker started.");
        using var scope = serviceProvider.CreateScope();
        await foreach (var message in queue.DequeueMessages(stoppingToken))
        {
            try
            {
                await ProcessMessageAsync(scope.ServiceProvider, message, stoppingToken);
                logger.LogInformation($"Successfully processed {message.GetType().Name} : {JsonSerializer.Serialize(message, message.GetType())}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to process {message.GetType().Name} : {JsonSerializer.Serialize(message, message.GetType())}");
            }
        }
        logger.LogInformation("Messaging background worker stopping.");
    }

    private async Task ProcessMessageAsync(IServiceProvider serviceProvider, IMessage message, CancellationToken stoppingToken)
    {
        var consumerType = typeof(IConsumer<>).MakeGenericType(message.GetType());
        var consumer = serviceProvider.GetService(consumerType);

        if (consumer == null)
        {
            var concreteConsumerType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .FirstOrDefault(p => consumerType.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);
            if (concreteConsumerType != null)
            {
                consumer = serviceProvider.GetRequiredService(concreteConsumerType);
            }
        }

        if (consumer == null)
        {
            throw new InvalidOperationException($"No consumer found for message type {message.GetType().Name}");
        }

        await ((dynamic)consumer).ConsumeAsync((dynamic)message, stoppingToken);
    }
}
