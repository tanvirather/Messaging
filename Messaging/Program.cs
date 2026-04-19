var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () =>
{
    return "Hello";
})
.WithName("GetWeatherForecast");

app.Run();

