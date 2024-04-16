using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lorebook.Convertor.Web.Api.Session.Post.SetSession
{
    public static class Endpoint
    {
        private static async Task<IActionResult> SetSession(IMediator mediator, [FromHeader(Name = "Session-Key")] Guid? sessionId, CancellationToken cancellationToken)
        {
            return new OkObjectResult(await mediator.Send(new Command { SessionId = sessionId }, cancellationToken));
        }


        public static IEndpointRouteBuilder AddSetSessionEndpoint(this IEndpointRouteBuilder builder)
        {
            builder.MapPost(V1.ApiUrl, SetSession);

            return builder;
        }
    }
}
