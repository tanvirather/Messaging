# Architecture Overview

This document describes the architecture of the Notification system in the `FlightOps` project. The system follows a decoupled, component-based approach for processing and sending notifications.

## High-Level Architecture

The notification system is built around a "Consumer" pattern. Each notification type (e.g., Welcome Email) has its own set of components that handle data retrieval, validation, content mapping, and delivery.

### Key Components

1.  **Message (`IMessage`)**: A simple DTO that contains the minimum information needed to trigger a notification (e.g., a `CustomerId`).
2.  **Consumer (`IConsumer<T>`)**: The orchestrator. It receives the message, coordinates with other components, and ultimately calls the `EmailService`.
3.  **Repository**: Responsible for fetching the full data required for the notification from the database, usually returning a specific **Model**.
4.  **Model**: A DTO representing the data needed to populate the notification template.
5.  **Validator**: Checks if the notification should be sent based on business rules or data availability. It throws a `ValidatorException` if validation fails.
6.  **Mapper (`BaseMapper`)**: Handles the transformation of the **Model** into a `MailMessage` by reading an HTML template and replacing placeholders.
7.  **EmailService**: A shared service responsible for the actual delivery of emails and text messages (via email gateways).

## Sequence Diagram

The following diagram illustrates the flow of a single notification request through the system.

```mermaid
sequenceDiagram
    participant Q as Message Queue / Caller
    participant C as FeatureConsumer
    participant R as FeatureRepository
    participant V as FeatureValidator
    participant M as FeatureMapper
    participant E as EmailService

    Q->>C: ConsumeAsync(Message)
    C->>R: Get(Id)
    R-->>C: Return Model
    C->>V: Validate(Message, Model)
    Note over V: Throws Exception if invalid

    C->>M: Map(Model)
    M->>M: Read HTML Template
    M->>M: Replace placeholders
    M-->>C: Return MailMessage
    C->>C: Set Recipient
    C->>E: SendEmailAsync(MailMessage)
    E-->>C: Success
```

## Component Relationships

The diagram below shows how the components are structured and how they interact.

```mermaid
classDiagram
    class IMessage {
        <<interface>>
    }
    class IConsumer~T~ {
        <<interface>>
        +ConsumeAsync(message, token)
    }
    class BaseMapper {
        #ReadTemplate(path)
        #CreateHtmlAsync(body, style)
    }

    class FeatureMessage {
        +Guid Id
    }
    class FeatureConsumer {
        -FeatureRepository repository
        -FeatureMapper mapper
        -FeatureValidator validator
        -EmailService emailService
        +ConsumeAsync(message, token)
    }
    class FeatureRepository {
        -NotificationContext context
        +Get(id) FeatureModel
    }
    class FeatureMapper {
        +Map(model) MailMessage
    }
    class FeatureValidator {
        +Validate(message, model)
    }
    class EmailService {
        +SendEmailAsync(mailMessage)
        +SendTextAsync(phone, body)
    }

    IMessage <|.. FeatureMessage
    IConsumer <|.. FeatureConsumer
    BaseMapper <|-- FeatureMapper
    FeatureConsumer ..> FeatureMessage : consumes
    FeatureConsumer --> FeatureRepository : uses
    FeatureConsumer --> FeatureValidator : uses
    FeatureConsumer --> FeatureMapper : uses
    FeatureConsumer --> EmailService : uses
    FeatureRepository ..> FeatureModel : returns
    FeatureMapper ..> FeatureModel : maps
```

## Implementation Details

- **Separation of Concerns**: Each class has a single responsibility.
- **Testability**: Components are designed to be unit-tested individually (see `test.md`).
- **Templates**: HTML templates are stored in feature-specific directories and processed by the `Mapper`.
- **Async/Await**: All I/O operations (database access, template reading, email sending) are asynchronous.
- **Resiliency**: The `EmailService` implements retry logic with exponential backoff.
- **Shared Assets**: `BaseMapper` uses shared CSS (`BaseComposer.css`) and HTML layout (`BaseComposer.html`) to ensure consistent branding.
