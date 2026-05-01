# General Coding Guidelines

This document outlines the general coding standards and guidelines to be followed across the `FlightOps` solution.

## Language and Framework
- Use **C# 14.0** and **.NET 10.0** features.
- Prefer **file-scoped namespaces** to reduce nesting.
- Use **primary constructors** for dependency injection and class initialization where appropriate.

## Naming Conventions
- **Classes, Methods, Properties**: Use `PascalCase`.
- **Private Fields**: Use `_camelCase` with an underscore prefix (if not using primary constructors).
- **Local Variables, Parameters**: Use `camelCase`.
- **Interfaces**: Prefix with `I` (e.g., `IMessage`).

## Code Structure
- **Feature-based Organization**: Group related components (Model, Repository, Mapper, Validator, Consumer) within a feature-specific folder.
- **Asynchronous Programming**: Use `async` and `await` for all I/O-bound operations.
- **Nullable Reference Types**: Enable and respect nullable reference types to avoid `NullReferenceException`.

## Dependency Injection
- Use constructor injection.
- Prefer `virtual` members in repositories and services to facilitate mocking in tests.

## Documentation
- Use XML documentation (`///`) for public APIs and complex logic.
- Keep comments concise and focused on the "why" rather than the "how".
