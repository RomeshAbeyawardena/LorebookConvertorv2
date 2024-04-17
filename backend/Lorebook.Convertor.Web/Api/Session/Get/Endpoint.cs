using Lorebook.Convertor.Web.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lorebook.Convertor.Web.Api.Session.Get;

public static class Endpoint
{
    private static async Task<IActionResult> GetSession(
        IMediator mediator, IHttpContextAccessor contextAccessor,
        Guid? sessionId, CancellationToken cancellationToken)
    {
        var httpContext = contextAccessor.HttpContext ?? throw new NotSupportedException();

        return Result.Ok(sessionId.HasValue 
            ? await mediator.Send(new Query {  SessionId = sessionId }, cancellationToken)
            : httpContext.GetSessionData());
    }
    
    public static IEndpointRouteBuilder AddGetSessionEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet(V1.ApiUrl, GetSession);
        return routeBuilder;
    }
}
