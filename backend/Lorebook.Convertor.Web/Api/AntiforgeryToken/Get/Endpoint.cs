using Lorebook.Convertor.Web.Api.Extensions;
using Lorebook.Convertor.Web.Api.Session;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lorebook.Convertor.Web.Api.AntiforgeryToken.Get;

public static class Endpoint
{
    private static async Task<IActionResult> GetAntiforgeryToken(TimeProvider timeProvider,
        IHttpContextAccessor httpContextAccessor, IMediator mediator, 
        Guid? sessionId, CancellationToken cancellationToken)
    {
        SessionData session;
        if (!sessionId.HasValue)
        {
            var context = httpContextAccessor.HttpContext 
                ?? throw new NotSupportedException();
            
            var sessionData = context.GetSessionData() 
                ?? throw new UnauthorizedAccessException("Unauthorised request");

            session = await mediator.Send(new Command
            {
                SessionId = sessionData.SessionId
            }, cancellationToken);

            return Result.Ok(timeProvider, session);
        }

        session = await mediator.Send(new Command { 
            SessionId = sessionId }, cancellationToken);
        return Result.Ok(timeProvider, session);
    }

    public const string ApiUrl = "api/aft";
    public static IEndpointRouteBuilder AddAntiforgeryEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(ApiUrl, GetAntiforgeryToken);
        return builder;
    }
}
