using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Zuhid.Notification.Shared;

namespace Zuhid.Notification.Tests.Shared;

public class SwaggerOperationFilterTest
{
    [Fact]
    public void Apply_ShouldModifyResponses()
    {
        // Arrange
        var filter = new SwaggerOperationFilter();
        var operation = new OpenApiOperation
        {
            Responses = new OpenApiResponses
            {
                { "200", new OpenApiResponse { Description = "OK" } }
            }
        };
        var context = new OperationFilterContext(null!, null!, null!, null!, null!);

        // Act
        filter.Apply((dynamic)operation, context);

        // Assert
        var responses = (dynamic)operation.Responses;
        Assert.False(responses.ContainsKey("200"));
        Assert.True(responses.ContainsKey("202"));
        Assert.True(responses.ContainsKey("400"));
        Assert.True(responses.ContainsKey("401"));
        Assert.True(responses.ContainsKey("500"));

        Assert.Equal("Accepted", (string)responses["202"].Description);
        Assert.Equal("Bad Request", (string)responses["400"].Description);
        Assert.Equal("Unauthorized", (string)responses["401"].Description);
        Assert.Equal("Internal Server Error", ((string)responses["500"].Description).Trim());
    }

    [Fact]
    public void Apply_WhenResponsesIsNull_ShouldNotThrow()
    {
        // Arrange
        var filter = new SwaggerOperationFilter();
        var operation = new OpenApiOperation
        {
            Responses = null!
        };

        var apiDescription = new ApiDescription();
        var context = new OperationFilterContext(apiDescription, null!, null!, null!, null!);

        // Act & Assert
        var exception = Record.Exception(() => { filter.Apply((dynamic)operation, context); });
        Assert.Null(exception);
    }
}
