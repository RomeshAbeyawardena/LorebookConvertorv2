using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lorebook.Convertor.Web.Api.AntiforgeryToken.Get;

public static class Endpoint
{
    private static async Task<IActionResult> GetAntiforgeryToken(IMediator mediator, [FromHeader(Name = "Session-Key")] Guid sessionId, CancellationToken cancellationToken)
    {
        var session = await mediator.Send(new Command { SessionId = sessionId }, cancellationToken);
        return new OkObjectResult(session.AntiforgeryToken);
    }

    public const string ApiUrl = "api/afg";
    public static IEndpointRouteBuilder AddAntiforgeryEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(ApiUrl, GetAntiforgeryToken);
        return builder;
    }
}
