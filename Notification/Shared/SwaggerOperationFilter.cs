using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Zuhid.Notification.Shared;

public class SwaggerOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Responses?.Remove("200");
        operation.Responses?.TryAdd("202", new OpenApiResponse { Description = "Accepted" });
        operation.Responses?.TryAdd("400", new OpenApiResponse { Description = "Bad Request" });
        operation.Responses?.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
        operation.Responses?.TryAdd("500", new OpenApiResponse { Description = "Internal Server Error" });
    }
}
