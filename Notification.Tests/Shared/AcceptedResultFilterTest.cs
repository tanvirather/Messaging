using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Zuhid.Notification.Shared;

namespace Zuhid.Notification.Tests.Shared;

public class AcceptedResultFilterTest
{
    [Fact]
    public void OnResultExecuting_WhenStatusCodeIs200_ShouldChangeTo202()
    {
        // Arrange
        var filter = new ResultFilter();
        var httpContext = new DefaultHttpContext();
        httpContext.Response.StatusCode = StatusCodes.Status200OK;

        var actionContext = new ActionContext(
            httpContext,
            new RouteData(),
            new ActionDescriptor()
        );

        var resultExecutingContext = new ResultExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new OkResult(),
            new object()
        );

        // Act
        filter.OnResultExecuting(resultExecutingContext);

        // Assert
        Assert.Equal(StatusCodes.Status202Accepted, httpContext.Response.StatusCode);
    }

    [Fact]
    public void OnResultExecuting_WhenStatusCodeIsNot200_ShouldNotChange()
    {
        // Arrange
        var filter = new ResultFilter();
        var httpContext = new DefaultHttpContext();
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        var actionContext = new ActionContext(
            httpContext,
            new RouteData(),
            new ActionDescriptor()
        );

        var resultExecutingContext = new ResultExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new BadRequestResult(),
            new object()
        );

        // Act
        filter.OnResultExecuting(resultExecutingContext);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, httpContext.Response.StatusCode);
    }
}
