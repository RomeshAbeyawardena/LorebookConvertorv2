using Lorebook.Convertor.Web.Api.Lorebook.Post.Convert;

namespace Lorebook.Convertor.Web.Api.Lorebook;

public static class Endpoints
{
    public static IEndpointRouteBuilder AddLorebookEndpoints(this IEndpointRouteBuilder builder)
    {
        return builder.AddConversionEndpoint();
    }
}
