using Lorebook.Convertor.Web.Api.Session.Post.SetSession;

namespace Lorebook.Convertor.Web.Api.Session;

public static class Endpoints
{
    public static IEndpointRouteBuilder AddSessionEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.AddSetSessionEndpoint();
        return builder;
    }
}
