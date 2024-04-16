using Lorebook.Convertor.Web.Api.Extensions;
using Lorebook.Convertor.Web.Api.Session;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lorebook.Convertor.Web.Api.AntiforgeryToken.Get;

public static class Endpoint
{
    private static async Task<IActionResult> GetAntiforgeryToken(IHttpContextAccessor httpContextAccessor, IMediator mediator, CancellationToken cancellationToken)
    {
        var context = httpContextAccessor.HttpContext ?? throw new NotSupportedException();
        var sessionData = context.GetSessionData() ?? throw new UnauthorizedAccessException("Unauthorised request");
        var session = await mediator.Send(new Command { SessionId = sessionData.SessionId }, cancellationToken);
        return new OkObjectResult(session);
    }

    public const string ApiUrl = "api/afg";
    public static IEndpointRouteBuilder AddAntiforgeryEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(ApiUrl, GetAntiforgeryToken);
        return builder;
    }
}
