# New Feature Implementation Guide (Based on Welcome Template)

Before you begin, please review the [General Coding Guidelines](General.md).

This guide provides instructions on how to build a new notification feature in the `Notification` project, using the `Welcome` feature as a template.

## Folder Structure
Create a new folder under `Notification/` for your feature (e.g., `Notification/NewFeature/`).

## Components to Implement

### 1. Message (`NewFeatureMessage.cs`)
Define the message that will be queued. It should implement `IMessage`.
```csharp
using Zuhid.Notification.Shared;
namespace Zuhid.Notification.NewFeature;

public class NewFeatureMessage : IMessage
{
    public Guid Id { get; set; }
}
```

### 2. Model (`NewFeatureModel.cs`)
Define the data structure used for mapping to the HTML template.
```csharp
namespace Zuhid.Notification.NewFeature;

public class NewFeatureModel
{
    public string Name { get; set; } = string.Empty;
    // Add other properties needed for the template
}
```

### 3. Repository (`NewFeatureRepository.cs`)
Implement the data retrieval logic.
```csharp
using Microsoft.EntityFrameworkCore;
using Zuhid.Notification.Shared;

namespace Zuhid.Notification.NewFeature;

public class NewFeatureRepository(NotificationContext context)
{
    public virtual async Task<NewFeatureModel?> Get(Guid id) => await context.SomeTable
        .Where(x => x.Id == id.ToString())
        .Select(x => new NewFeatureModel
        {
            Name = x.Name ?? string.Empty,
        })
        .FirstOrDefaultAsync();
}
```

### 4. Mapper (`NewFeatureMapper.cs`)
Inherit from `BaseMapper` to handle template reading and string replacement.
```csharp
using Zuhid.Notification.Shared;

namespace Zuhid.Notification.NewFeature;

public class NewFeatureMapper() : BaseMapper("NewFeature")
{
    public virtual async Task<(string Subject, string Body)> Map(NewFeatureModel model)
    {
        var subject = "Subject for New Feature";
        var body = (await ReadTemplate("NewFeature.html"))
            .Replace("{{name}}", model.Name);
        
        return (subject, await CreateHtmlAsync(body));
    }
}
```

### 5. Validator (`NewFeatureValidator.cs`)
Implement validation logic before sending the notification.
```csharp
namespace Zuhid.Notification.NewFeature;

public class NewFeatureValidator(ILogger<NewFeatureValidator> logger)
{
    public virtual bool IsValid(NewFeatureMessage message, NewFeatureModel? model)
    {
        if (model == null)
        {
            logger.LogWarning("Data not found for {Id}.", message.Id);
            return false;
        }
        return true;
    }
}
```

### 6. Consumer (`NewFeatureConsumer.cs`)
Orchestrate the process: retrieve data, validate, map, and send.
```csharp
using Zuhid.Notification.NewFeature;
using Zuhid.Notification.Shared;

namespace Zuhid.Notification.NewFeature;

public class NewFeatureConsumer(
    EmailService emailService,
    NewFeatureRepository repository,
    NewFeatureMapper mapper,
    NewFeatureValidator validator) : IConsumer<NewFeatureMessage>
{
    public async Task ConsumeAsync(NewFeatureMessage message, CancellationToken stoppingToken)
    {
        var data = await repository.Get(message.Id);
        if (validator.IsValid(message, data))
        {
            var (subject, body) = await mapper.Map(data!);
            await emailService.SendEmailAsync(subject, body, "recipient@example.com");
        }
    }
}
```

### 7. HTML Template (`NewFeature.html`)
Create an HTML file in the feature folder. Use `{{property_name}}` for placeholders.

### 8. Controller Endpoint (`EmailController.cs`)
Add a new `HttpPost` endpoint to the `EmailController` (located in `Notification/Controllers/`) to queue the message.
```csharp
[HttpPost("NewFeature")]
public virtual async Task<IActionResult> NewFeature([FromBody] NewFeatureMessage message) => await QueueMessage(message);
```

## Testing
Do not write any unit tests or integration tests for the new feature.
