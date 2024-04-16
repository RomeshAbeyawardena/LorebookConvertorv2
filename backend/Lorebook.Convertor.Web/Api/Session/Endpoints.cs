using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Lorebook.Convertor.Web.Api.Session;

public static class Endpoints
{
    private static async Task<IActionResult> SetSession(IMediator mediator, CancellationToken cancellationToken)
    {
       
    }

    public static IEndpointRouteBuilder AddSession(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("api/session", SetSession);

        return builder;
    }
}
