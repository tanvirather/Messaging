# Zuhid Messaging Project

This project provides messaging capabilities including order management and email services. It uses Entity Framework Core for data persistence and PostgreSQL as the database.

## Database Setup and Migrations

Follow these steps to set up the database and manage migrations.

### Prerequisites

Ensure you have the EF Core CLI tools installed:

```powershell
dotnet tool install --global dotnet-ef
```

### 1. Creating a Migration

When you make changes to the data models in the `Messaging` project, you need to create a new migration. Run the following command from the solution root:

```powershell
dotnet ef migrations add <MigrationName> --project Messaging --startup-project Messaging.Api --context MessagingContext
```

Example:
```powershell
dotnet ef migrations add InitialCreate --project Messaging --startup-project Messaging.Api --context MessagingContext
```

### 2. Running Migrations (Updating the Database)

To apply the migrations and create or update the database schema, run:

```powershell
dotnet ef database update --project Messaging --startup-project Messaging.Api --context MessagingContext
```

### 3. Generating a SQL Script (Optional)

If you prefer to generate a SQL script to run manually against your database:

```powershell
dotnet ef migrations script --project Messaging --startup-project Messaging.Api --context MessagingContext
```

## Configuration

The connection string for the database is located in `Messaging.Api/appsettings.Development.json`. Ensure the server and credentials match your PostgreSQL environment.

```json
"ConnectionStrings": {
  "Messaging": "Server=localhost;Database=messaging;[postgres_credential];TrustServerCertificate=True;"
}
```

## Seeding Data

The `MessagingContext` is configured to load seed data from CSV files located in `Messaging/Dataload/`. This data is automatically seeded during migration/database update via `builder.LoadCsvData`.
