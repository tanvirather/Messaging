# Testing Guide for New Features

Refer to the [General Coding Guidelines](General.md) for overall coding standards.

This guide provides instructions on how to write tests for a new notification feature, using the `Welcome` feature as a template.

## Test Project Structure
Place your tests in the `Notification.Tests` project, within a folder named after your feature (e.g., `Notification.Tests/NewFeature/`).

## Data for Tests
If your tests require mock data, add a CSV file to the `Notification.Tests/Dataload` folder.
Ensure that:
1. The CSV filename matches the one used in `DataLoader.LoadCsv<T>("YourData.csv")`.
2. The CSV headers match the property names of your entity class.
3. The CSV file's **Copy to Output Directory** property is set to **Copy if newer** (this is usually handled by the wildcard in the `.csproj` file).

## Types of Tests to Implement

### 1. Repository Tests (`NewFeatureRepositoryTest.cs`)
Use the `DataLoader` utility to mock the database context with CSV data.

```csharp
using Zuhid.Notification.Entities;
using Zuhid.Notification.NewFeature;

namespace Zuhid.Notification.Tests.NewFeature
{
    public class NewFeatureRepositoryTest
    {
        private readonly NewFeatureRepository _repository;

        public NewFeatureRepositoryTest()
        {
            var dataLoader = new DataLoader();
            // Load mock data from a CSV file
            var data = DataLoader.LoadCsv<YourEntity>("YourData.csv");
            dataLoader.SetupDbSet(data, c => c.YourEntities);
            _repository = new NewFeatureRepository(dataLoader.Context);
        }

        [Fact]
        public async Task Get_WhenRecordExists_ShouldReturnModel()
        {
            // Act
            var result = await _repository.Get(new Guid("..."));

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Expected Name", result.Name);
        }

        [Fact]
        public async Task Get_WhenRecordDoesNotExist_ShouldReturnNull()
        {
            // Act
            var result = await _repository.Get(new Guid());

            // Assert
            Assert.Null(result);
        }
    }
}
```

### 2. Mapper Tests (`NewFeatureMapperTest.cs`)
Verify that the mapper correctly replaces placeholders in the HTML template and returns a `MailMessage`.

```csharp
using Zuhid.Notification.NewFeature;

namespace Zuhid.Notification.Tests.NewFeature
{
    public class NewFeatureMapperTest
    {
        [Fact]
        public async Task Map_ShouldReturnExpectedSubjectAndBody()
        {
            // Arrange
            var mapper = new NewFeatureMapper();
            var model = new NewFeatureModel
            {
                Name = "Test User",
                // Set other properties
            };

            // Act
            var mailMessage = await mapper.Map(model);

            // Assert
            Assert.Equal("Expected Subject", mailMessage.Subject);
            Assert.Contains("Test User", mailMessage.Body);
            // Ensure no placeholders remain
            Assert.DoesNotContain("{{", mailMessage.Body);
            Assert.DoesNotContain("}}", mailMessage.Body);
            Assert.True(mailMessage.IsBodyHtml);
        }
    }
}
```

## General Guidelines
- **Use `Fact` for simple tests**: Most unit tests should be marked with the `[Fact]` attribute.
- **Naming Convention**: Follow the pattern `MethodName_StateUnderTest_ExpectedBehavior`.
- **Assert Patterns**:
    - Use `Assert.NotNull(result)` for existence checks.
    - Use `Assert.Equal(expected, actual)` for property values.
    - Use `Assert.Contains(substring, body)` to verify template replacements.
    - Use `Assert.DoesNotContain("{{", body)` to ensure all placeholders were processed.
