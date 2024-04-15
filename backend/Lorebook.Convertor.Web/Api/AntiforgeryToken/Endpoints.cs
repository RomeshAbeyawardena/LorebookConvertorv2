using Lorebook.Convertor.Web.Api.AntiforgeryToken.Get;

namespace Lorebook.Convertor.Web.Api.AntiforgeryToken;

public static class Endpoints
{
    public static IEndpointRouteBuilder AddAntiForgeryTokenEndpoints(this IEndpointRouteBuilder builder)
    {
        return builder.AddAntiforgeryEndpoint();
    }
}
