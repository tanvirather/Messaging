using Microsoft.Extensions.Configuration;
using Zuhid.Notification.Shared;

namespace Zuhid.Notification.Tests.Shared;

public class AppSettingTest
{
    [Fact]
    public void Constructor_ShouldBindValuesFromConfiguration()
    {
        // Arrange
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "AppSettings:Name", "TestApp" },
            { "AppSettings:Version", "1.0.0" },
            { "AppSettings:CorsOrigin", "http://localhost:3000" },
            { "AppSettings:Smtp:Host", "localhost" },
            { "AppSettings:Smtp:Port", "1025" },
            { "AppSettings:Smtp:From", "no-reply@test.com" },
            { "AppSettings:Smtp:RetryCount", "3" },
            { "AppSettings:Smtp:RetryInterval", "00:00:05" },
            { "ConnectionStrings:Notification", "Host=localhost;Database=Notification;Username=[postgres_credential]" },
            { "postgres_credential", "myuser:mypassword" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        // Act
        var appSetting = new AppSetting(configuration);

        // Assert
        Assert.Equal("TestApp", appSetting.Name);
        Assert.Equal("1.0.0", appSetting.Version);
        Assert.Equal("http://localhost:3000", appSetting.CorsOrigin);
        Assert.Equal("localhost", appSetting.Smtp.Host);
        Assert.Equal(1025, appSetting.Smtp.Port);
        Assert.Equal("no-reply@test.com", appSetting.Smtp.From);
        Assert.Equal(3, appSetting.Smtp.RetryCount);
        Assert.Equal(TimeSpan.FromSeconds(5), appSetting.Smtp.RetryInterval);
        Assert.Equal("Host=localhost;Database=Notification;Username=myuser:mypassword", appSetting.ConnectionStrings.Notification);
    }
}
