// using System.Reflection;
using Zuhid.Base;
using Zuhid.Messaging.Mappers;

namespace Zuhid.Messaging.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplicationExtension.AddServices(args);
        // Assembly.GetAssembly(typeof(MessagingContext))!.GetTypes().Where(s =>
        //     s.Name.EndsWith("Repository")
        //     || s.Name.EndsWith("Mapper")
        //     || s.Name.EndsWith("Validator")
        //   )
        //   .ToList()
        //   .ForEach(item => builder.Services.AddScoped(item));
        builder.Services.AddScoped<IOrderMapper, OrderMapper>();

        var appSetting = new AppSetting(builder.Configuration);
        builder.Services.AddSingleton(appSetting);
        builder.Services.AddScoped<IEmailService, EmailService>();

        // builder.AddDatabase<FlightOpsContext, FlightOpsContext>(appSetting.ConnectionStrings.FlightOps);
        var app = builder.BuildServices();
        app.Run();
    }
}
