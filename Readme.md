# Notification Service

A robust notification system built with .NET 10, featuring email delivery, background processing, and a RESTful API.

## Project Structure

- **Notification**: The core library containing business logic, entities, and services for handling notifications.
- **Notification.Api**: ASP.NET Core Web API that provides endpoints for triggering notifications and manages background processing.
- **Notification.Tests**: Comprehensive unit tests for the notification logic.
- **Base**: Shared infrastructure and base classes used across the project.

## Key Features

- **Email Service**: Reliable email delivery with built-in retry logic and exponential backoff.
- **Background Processing**: Efficient handling of notification queues using background services.
- **Scalable Architecture**: Built on .NET 10 with a focus on modularity and testability.

## Instructions

For instructions on development practices and standards used in this project, please refer to:

- [Coding Instructions](Instructions/Code.md)
- [Unit Testing Instructions](Instructions/UnitTest.md)

## API Documentation

For details on how to use the available API endpoints, see:

- [EmailController Usage](Notification/Controllers/EmailController.md)

## Getting Started

1. **Install mailpit**: `docker run -d --restart unless-stopped --name=mailpit -p 8025:8025 -p 1025:1025 axllent/mailpit`
2. **Database Setup**: Configure the connection string in `appsettings.json`.
3. **Run the API**: Start the `Notification.Api` project.
4. **Tests**: Run the tests in `Notification.Tests` to ensure everything is working correctly.
