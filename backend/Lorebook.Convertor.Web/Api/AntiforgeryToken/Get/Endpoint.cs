using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace Lorebook.Convertor.Web.Api.AntiforgeryToken.Get;

public static class Endpoint
{
    private static async Task<IActionResult> GetAntiforgeryToken(IHttpContextAccessor httpContext,
        IAntiforgery antiforgery, IMemoryCache memoryCache)
    {
        await Task.CompletedTask;
        var context = httpContext.HttpContext ?? throw new NullReferenceException();

        // Check if the token is already stored in the session
        var token = memoryCache.Get<string>("AntiforgeryToken");
        if (token == null)
        {
            // Generate a new token if it doesn't exist in the session
            var tokens = antiforgery.GetTokens(context);
            token = tokens.RequestToken;

            // Store the token in the session
            memoryCache.Set("AntiforgeryToken", token);
        }

        return new OkObjectResult(token);
    }

    public const string ApiUrl = "api/afg";
    public static IEndpointRouteBuilder AddAntiforgeryEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapGet(ApiUrl, GetAntiforgeryToken);
        return builder;
    }
}
