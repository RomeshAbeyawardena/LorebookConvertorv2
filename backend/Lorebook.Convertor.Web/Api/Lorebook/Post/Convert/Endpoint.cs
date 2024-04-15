using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Lorebook.Convertor.Web.Api.Lorebook.Post.Convert;

public static class Endpoint
{
    private static async Task<IActionResult> Convert(IHttpContextAccessor httpContext, IMediator mediator, 
        [FromForm]string version)
    {
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
            .MapPost(EndpointUrl, Convert);
        return builder;
    }
}
