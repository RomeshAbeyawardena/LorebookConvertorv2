using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace Lorebook.Convertor.Web.Api.AntiforgeryToken.Get;

public static class Endpoint
{
    private static async Task<IActionResult> GetAntiforgeryToken(IHttpContextAccessor httpContext,
        IAntiforgery antiforgery)
    {
        await Task.CompletedTask;
        var context = httpContext.HttpContext ?? throw new NullReferenceException();
        var tokens = antiforgery.GetAndStoreTokens(context);
        return new OkObjectResult(tokens);

    }

    public const string ApiUrl = "api/afg";
    public static IEndpointRouteBuilder AddAntiforgeryEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(ApiUrl, GetAntiforgeryToken);
        return builder;
    }
}
