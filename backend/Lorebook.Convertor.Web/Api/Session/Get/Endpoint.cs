using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lorebook.Convertor.Web.Api.Session.Get;

public static class Endpoint
{
    private static async Task<IActionResult> GetSession(IMediator mediator, 
        Guid? sessionId,
        CancellationToken cancellationToken)
    {
        return new OkObjectResult(sessionId.HasValue 
            ? await mediator.Send(new Query {  SessionId = sessionId }, cancellationToken)
            : new SessionData()
            );
    }
    
    public static IEndpointRouteBuilder AddGetSessionEndpoint(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet(V1.ApiUrl, GetSession);
        return routeBuilder;
    }
}
