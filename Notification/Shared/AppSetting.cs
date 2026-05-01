namespace Zuhid.Notification.Shared;

public class AppSetting
{
    public AppSetting(IConfiguration configuration)
    {
        configuration.GetSection("AppSettings").Bind(this);
        ConnectionStrings = new ConnectionString
        {
            Notification = ReplaceCredential(configuration, "Notification")
        };
    }

    public string Name { get; init; } = default!;
    public string Version { get; init; } = default!;
    public string CorsOrigin { get; init; } = default!;
    public ConnectionString ConnectionStrings { get; init; } = default!;
    public SmtpSetting Smtp { get; init; } = default!;

    public class ConnectionString
    {
        public string Notification { get; init; } = default!;
    }

    public class SmtpSetting
    {
        public string Host { get; init; } = default!;
        public int Port { get; init; }
        public string From { get; init; } = default!;
        public int RetryCount { get; init; }
        public TimeSpan RetryInterval { get; init; }
    }

    private static string ReplaceCredential(IConfiguration configuration, string connString)
    {
        return configuration.GetConnectionString(connString)!
          .Replace("[postgres_credential]", configuration.GetValue<string>("postgres_credential"), StringComparison.Ordinal);
    }
}
