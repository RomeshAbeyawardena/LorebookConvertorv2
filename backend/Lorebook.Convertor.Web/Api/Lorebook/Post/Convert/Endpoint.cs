using Lorebook.Convertor.Web.Api.Session.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lorebook.Convertor.Web.Api.Lorebook.Post.Convert;

public static class Endpoint
{
    private static async Task<IActionResult> Convert(IHttpContextAccessor httpContext, IMediator mediator, TimeProvider timeProvider,
         [FromHeader(Name = "x-aft")] string antiforgeryToken, [FromForm]string version, 
        CancellationToken cancellationToken)
    {
        //we should not accept anything from the client until these checks are done
        var session = await mediator.Send(new Query { SessionId = sessionId }, cancellationToken) ?? throw new UnauthorizedAccessException();
        
        if (session.Expires < timeProvider.GetUtcNow())
        {
            throw new UnauthorizedAccessException("Session expired");
        }

        if (session.AntiforgeryToken != antiforgeryToken)
        {
            throw new InvalidOperationException("Antiforgery token validation failed");
        }

        var context = httpContext.HttpContext ?? throw new NotSupportedException();
        var file = context.Request.Form.Files[0] ?? throw new NullReferenceException();
        return new OkObjectResult(await mediator.Send(new Command { 
            File = file, 
            Version = version 
        }));
    }

    public const string EndpointUrl = "api/convert";
    public static IEndpointRouteBuilder AddConversionEndpoint(this IEndpointRouteBuilder builder)
    {
        builder
            .MapPost(EndpointUrl, Convert).DisableAntiforgery();
        return builder;
    }
}
