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
    [RequiredGuid]
    public Guid CustomerId { get; set; }
}
```

### 2. Model (`NewFeatureModel.cs`)
Define the data structure used for mapping to the HTML template.
```csharp
namespace Zuhid.Notification.NewFeature;

public class NewFeatureModel
{
    public Guid CustomerId { get; set; }
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
    public virtual async Task<NewFeatureModel?> Get(Guid customerId) => await context.Customer
        .Where(x => x.Id == customerId)
        .Select(x => new NewFeatureModel
        {
            CustomerId = x.Id,
            Name = x.FirstName ?? string.Empty,
        })
        .FirstOrDefaultAsync();
}
```

### 4. Mapper (`NewFeatureMapper.cs`)
Inherit from `BaseMapper` to handle template reading and string replacement.
```csharp
using System.Net.Mail;
using Zuhid.Notification.Shared;

namespace Zuhid.Notification.NewFeature;

public class NewFeatureMapper() : BaseMapper("NewFeature")
{
    public virtual async Task<MailMessage> Map(NewFeatureModel model)
    {
        var subject = "Subject for New Feature";
        var body = (await ReadTemplate("NewFeature.html"))
            .Replace("{{name}}", model.Name);

        return new MailMessage
        {
            Subject = subject,
            Body = await CreateHtmlAsync(body),
            IsBodyHtml = true
        };
    }
}
```

### 5. Validator (`NewFeatureValidator.cs`)
Implement validation logic before sending the notification.
```csharp
using Zuhid.Notification.Shared;

namespace Zuhid.Notification.NewFeature;

public class NewFeatureValidator
{
    public virtual void Validate(NewFeatureMessage message, NewFeatureModel? model)
    {
        if (model == null)
        {
            throw new ValidatorException([$"Data not found for {message.CustomerId}."]);
        }
    }
}
```

### 6. Consumer (`NewFeatureConsumer.cs`)
Orchestrate the process: retrieve data, validate, map, and send.
```csharp
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
        var data = await repository.Get(message.CustomerId);
        validator.Validate(message, data);
        var mailMessage = await mapper.Map(data!);
        mailMessage.To.Add("recipient@example.com");
        await emailService.SendEmailAsync(mailMessage);
    }
}
```

### 7. HTML Template (`NewFeature.html`)
Create an HTML file in the feature folder. Use `{{property_name}}` for placeholders.

### 8. Controller Endpoint (`EmailController.cs`)
Add a new `HttpPost` endpoint to the `EmailController` (located in `Notification/Controllers/`) to queue the message.
```csharp
[HttpPost("NewFeature")]
public async Task NewFeature([FromBody] NewFeatureMessage message) => await emailQueue.QueueMessage(message);
```

## Testing
Do not write any unit tests or integration tests for the new feature.
