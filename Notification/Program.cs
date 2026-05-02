using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Reflection;
using Zuhid.Notification.Shared;

namespace Zuhid.Notification;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = AddServices(args);
        var appSetting = new AppSetting(builder.Configuration);
        builder.Services.AddSingleton(appSetting);

        builder.Services.AddDbContext<NotificationContext>(options => options
          .UseNpgsql(appSetting.ConnectionStrings.Notification)
          .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
        );
        var app = BuildServices(builder);
        app.Run();
    }

    public static WebApplicationBuilder AddServices(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Assembly.GetExecutingAssembly().GetTypes().Where(s => s.IsClass && !s.IsAbstract && (
                s.Name.EndsWith("Consumer")
                || s.Name.EndsWith("Mapper")
                || s.Name.EndsWith("Repository")
                || s.Name.EndsWith("Validator")
            )).ToList()
            .ForEach(item => builder.Services.AddScoped(item));

        builder.Services
            .AddCors(options => options.AddPolicy(name: "CorsPolicy", policy => policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            ))
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Notification", Version = "v1" });
                options.OperationFilter<SwaggerOperationFilter>();
            })
            .AddHostedService<NotificationBackgroundService>()
            .AddScoped<EmailService>()
            .AddSingleton<ISmtpClient, SmtpClientWrapper>()
            .AddSingleton<NotificationQueue>()
            .AddControllers(options =>
            {
                options.Filters.Add<ResultFilter>();
            });
        return builder;
    }

    public static WebApplication BuildServices(this WebApplicationBuilder builder)
    {
        var app = builder.Build();
        app.UseCors("CorsPolicy");
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification");
            c.RoutePrefix = "swagger"; // Set Swagger UI at /swagger
        });
        app.MapGet("/", async context => await context.Response.WriteAsync("""
    <html>
    <body style='padding:100px 0;text-align:center;font-size:xxx-large;'>
      <a href='./swagger/index.html'>View Swagger</a>
    </body>
    </html>
    """));
        app.MapControllers();
        return app;
    }
}
