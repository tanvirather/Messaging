using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Zuhid.Base;

public static class WebApplicationExtension
{
    public static WebApplicationBuilder AddServices(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Services.AddCors(options => options.AddPolicy(name: "CorsPolicy", policy => policy
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()));
        return builder;
    }

    public static WebApplication BuildServices(this WebApplicationBuilder builder)
    {
        var app = builder.Build();
        app.UseCors("CorsPolicy");
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
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

    public static void AddDatabase<TContext>(this WebApplicationBuilder builder, string connectionString)
      where TContext : DbContext
    {
        builder.Services.AddDbContext<TContext>(options => options
          .UseNpgsql(connectionString)
          .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking) // setting to no tracking to improve performance
        //   .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
        );
    }
}
