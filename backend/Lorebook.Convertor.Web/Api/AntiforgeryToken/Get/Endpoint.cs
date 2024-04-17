using Lorebook.Convertor.Web.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lorebook.Convertor.Web.Api.AntiforgeryToken.Get;

public static class Endpoint
{
    private static async Task<IActionResult> GetAntiforgeryToken(
        IHttpContextAccessor httpContextAccessor, IMediator mediator, 
        Guid? sessionId, CancellationToken cancellationToken)
    {
        if (!sessionId.HasValue)
        {
            var context = httpContextAccessor.HttpContext ?? throw new NotSupportedException();
            var sessionData = context.GetSessionData() ?? throw new UnauthorizedAccessException("Unauthorised request");

            return new OkObjectResult(sessionData);
        }

        var session = await mediator.Send(new Command { 
            SessionId = sessionId }, cancellationToken);
        return Result.Ok(session);
    }

    public const string ApiUrl = "api/afg";
    public static IEndpointRouteBuilder AddAntiforgeryEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(ApiUrl, GetAntiforgeryToken);
        return builder;
    }
}
