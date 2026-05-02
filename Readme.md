# Notification Service

A robust notification system built with .NET 10, featuring email delivery, background processing, and a RESTful API.

## Project Structure

- **Notification**: The core library containing business logic, entities, and services for handling notifications.
- **Notification.Tests**: Comprehensive unit tests for the notification logic.
- **Base**: Shared infrastructure and base classes used across the project.

## Key Features

- **Email Service**: Reliable email delivery with built-in retry logic and exponential backoff.
- **Background Processing**: Efficient handling of notification queues using background services.
- **Scalable Architecture**: Built on .NET 10 with a focus on modularity and testability.

## Documentation

For detailed information about the system architecture, coding standards, and testing practices, please refer to the following documents:

- [Architecture Overview](Wiki/Architecture.md)
- [Architectural Rationale](Wiki/Rationale.md)
- [Coding Guidelines](Wiki/Code.md)
- [General Principles](Wiki/General.md)
- [Testing Guide](Wiki/Test.md)

## Feature Documentation

For information on specific notification features, see:

- [Welcome](Notification/Welcome/Readme.md)

## Getting Started

1. **Install mailpit**: `docker run -d --restart unless-stopped --name=mailpit -p 8025:8025 -p 1025:1025 axllent/mailpit`
2. **Database Setup**: Configure the connection string in `appsettings.json`.
3. **Run the API**: Start the `Notification` project.
4. **Tests**: Run the tests in `Notification.Tests` to ensure everything is working correctly.
