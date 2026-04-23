using Zuhid.Base;
using Zuhid.Messaging.Consumers;
using Zuhid.Messaging.Composers;
using Zuhid.Messaging.Messages;
using Zuhid.Messaging.Repositories;
using Zuhid.Messaging.Validators;
using System.Reflection;

namespace Zuhid.Messaging.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplicationExtension.AddServices(args);
        var appSetting = new AppSetting(builder.Configuration);
        builder.Services.AddSingleton(appSetting);

        Assembly.GetAssembly(typeof(MessagingContext))!.GetTypes().Where(s =>
            s.IsClass && (
              s.Name.EndsWith("Composer")
              || s.Name.EndsWith("Consumer")
              || s.Name.EndsWith("Repository")
              || s.Name.EndsWith("Validator")
            )
          )
          .ToList()
          .ForEach(item => builder.Services.AddScoped(item));


        builder.Services.AddScoped<EmailService>();
        builder.Services.AddSingleton<MessagingQueue>();
        builder.Services.AddHostedService<MessagingBackgroundService>();
        builder.AddDatabase<MessagingContext>(appSetting.ConnectionStrings.Messaging);
        var app = builder.BuildServices();
        app.Run();
    }
}
