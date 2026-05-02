# Welcome Feature

The `WelcomeConsumer` is responsible for orchestrating the process of sending a welcome email to new customers.

## Workflow

When a `WelcomeMessage` is received, the `WelcomeConsumer` performs the following steps:

1.  **Data Retrieval**: Uses the `WelcomeRepository` to fetch customer information (`WelcomeModel`) based on the `CustomerId` provided in the message.
2.  **Validation**: Invokes the `WelcomeValidator` to ensure that the message and the fetched data meet all business requirements before proceeding.
3.  **Mapping**: Uses the `WelcomeMapper` to transform the `WelcomeModel` into a `MailMessage` by applying the data to the `Welcome.html` template.
4.  **Delivery**: Calls the `EmailService` to send the generated email to the customer.

## Components

- `WelcomeConsumer.cs`: The main orchestrator.
- `WelcomeMessage.cs`: The trigger message containing the `CustomerId`.
- `WelcomeRepository.cs`: Handles data access for welcome-related information.
- `WelcomeValidator.cs`: Contains business rules to validate the welcome notification.
- `WelcomeMapper.cs`: Maps the data model to the HTML template.
- `Welcome.html`: The email template with placeholders.
- `WelcomeModel.cs`: The data transfer object used for mapping.
