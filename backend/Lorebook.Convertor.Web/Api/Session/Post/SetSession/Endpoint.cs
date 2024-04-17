using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lorebook.Convertor.Web.Api.Session.Post.SetSession
{
    public static class Endpoint
    {
        private static async Task<IActionResult> SetSession(TimeProvider timeProvider,
            IMediator mediator, [FromHeader(Name = "Session-Key")] Guid? sessionId, CancellationToken cancellationToken)
        {
            return Result.Ok(timeProvider, await mediator.Send(
                    new Command { SessionId = sessionId }, cancellationToken));
        }


        public static IEndpointRouteBuilder AddSetSessionEndpoint(this IEndpointRouteBuilder builder)
        {
            builder.MapPost(V1.ApiUrl, SetSession);

            return builder;
        }
    }
}
