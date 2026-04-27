# Instructions to create a new Email (e.g., Order)

Follow these steps to implement a new email notification type in the system, using the `Order` email as a reference.

## 1. Create the Composer
Create a class in `Notification/Composers/`. Inherit from `BaseEmailComposer` to use helper methods like `ReadTemplate` and `CreateHtmlAsync`.

**Example: `NewEmailComposer.cs`**
```csharp
public class NewEmailComposer : BaseEmailComposer
{
    public virtual async Task<(string Subject, string Body)> Map(EntityModel entity)
    {
        var subject = "Your Subject Here";
        var body = (await ReadTemplate("NewEmailTemplate.html"))
            .Replace("{placeholder}", entity.Value);
        return (subject, await CreateHtmlAsync(body));
    }
}
```

## 2. Create the Model
Create a model in `Notification/Models/` to hold the data used by the composer and fetched by the repository.

**Example: `EntityModel.cs`**
```csharp
public class EntityModel
{
    public Guid Id { get; set; }
    public string Value { get; set; } = string.Empty;
    public CustomerModel? Customer { get; set; }
}
```

## 3. Create the Repository
Create a repository class in `Notification/Repositories/` to fetch and project data from the database into models. The repository should accept `NotificationContext` in its constructor.

**Example: `NewEntityRepository.cs`**
```csharp
public class NewEntityRepository(NotificationContext context)
{
    public virtual async Task<EntityModel?> GetByIdAsync(Guid entityId) => await context.Entities
        .Where(e => e.Id == entityId)
        .Select(e => new EntityModel
        {
            Id = e.Id,
            Value = e.Value,
            Customer = e.Customer != null ? new CustomerModel
            {
                Email = e.Customer.Email
            } : null
        })
        .FirstOrDefaultAsync();
}
```

## 4. Define the Message
Create a message class in `Notification/Messages/`. This class should implement `IMessage` (if applicable) and contain the data needed to identify the entity (e.g., `OrderId`).

**Example: `NewEmailMessage.cs`**
```csharp
public class NewEmailMessage : IMessage
{
    public Guid EntityId { get; set; }
}
```

## 5. Update the Controller
To add a new email type to the `EmailController` in `Notification.Api/Controllers/`, add a new `HttpPost` method that accepts the message and calls `QueueMessage`.

**Example: `EmailController.cs`**
```csharp
[HttpPost("NewEmail")]
public virtual async Task<IActionResult> NewEmail([FromBody] NewEmailMessage message) => await QueueMessage(message);
```

## 6. Create the Validator (Optional but Recommended)
Create a validator in `Notification/Validators/` to ensure the data is valid before processing.

**Example: `NewEmailValidator.cs`**
```csharp
public class NewEmailValidator(ILogger<NewEmailValidator> logger)
{
    public virtual bool IsValid(NewEmailMessage message, EntityModel? entity)
    {
        if (entity == null)
        {
            logger.LogWarning("Entity {EntityId} not found.", message.EntityId);
            return false;
        }
        return true;
    }
}
```

## 7. Create the HTML Template
Add a new `.html` template in `Notification/Templates/`. Use placeholders like `{variableName}` for dynamic content.

## 8. Create the Consumer
Create a consumer in `Notification/Consumers/` that implements `IConsumer<NewEmailMessage>`. This class orchestrates the validation, mapping, and sending of the email.

**Example: `NewEmailConsumer.cs`**
```csharp
public class NewEmailConsumer(
    EmailService emailService,
    NewEntityRepository repository,
    NewEmailComposer composer,
    NewEmailValidator validator) : IConsumer<NewEmailMessage>
{
    public async Task ConsumeAsync(NewEmailMessage message, CancellationToken stoppingToken)
    {
        var entity = await repository.GetByIdAsync(message.EntityId);
        if (validator.IsValid(message, entity))
        {
            var (subject, body) = await composer.Map(entity!);
            await emailService.SendEmailAsync(entity!.Customer!.Email, subject, body);
        }
    }
}
```
