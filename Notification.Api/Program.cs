using Zuhid.Base;
using System.Reflection;

namespace Zuhid.Notification.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplicationExtension.AddServices(args);
        var appSetting = new AppSetting(builder.Configuration);
        builder.Services.AddSingleton(appSetting);

        Assembly.GetAssembly(typeof(NotificationContext))!.GetTypes().Where(s =>
            s.IsClass && (
              s.Name.EndsWith("Composer")
              || s.Name.EndsWith("Consumer")
              || s.Name.EndsWith("Repository")
              || s.Name.EndsWith("Validator")
            )
          )
          .ToList()
          .ForEach(item => builder.Services.AddScoped(item));


        builder.Services.AddSingleton<ISmtpClient, SmtpClientWrapper>();
        builder.Services.AddScoped<EmailService>();
        builder.Services.AddSingleton<NotificationQueue>();
        builder.Services.AddHostedService<NotificationBackgroundService>();
        builder.AddDatabase<NotificationContext>(appSetting.ConnectionStrings.Notification);
        var app = builder.BuildServices();
        app.Run();
    }
}
