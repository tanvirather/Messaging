using Zuhid.Messaging.Messages;

namespace Zuhid.Messaging.Consumers;

public interface IWelcomeConsumer : IConsumer<WelcomeMessage>
{
}

public class WelcomeConsumer(
    EmailService emailService,
    ILogger<WelcomeConsumer> logger) : IWelcomeConsumer
{
    public async Task ConsumeAsync(WelcomeMessage message, CancellationToken stoppingToken)
    {
        try
        {
            var subject = $"Welcome to Zuhid Messaging, {message.Name}!";
            var body = $"Hi {message.Name}, we're glad to have you on board.";

            await emailService.SendEmailAsync(message.Email, subject, body);
            logger.LogInformation("Sent welcome email to {Email}", message.Email);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing welcome email for {Email}.", message.Email);
        }
    }
}
