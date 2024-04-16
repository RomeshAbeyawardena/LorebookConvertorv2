﻿using Lorebook.Convertor.Web.Api.Session.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lorebook.Convertor.Web.Api.Lorebook.Post.Convert;

public static class Endpoint
{
    private static async Task<IActionResult> Convert(IHttpContextAccessor httpContext, IMediator mediator,
         [FromForm]string version, 
        CancellationToken cancellationToken)
    {
        var context = httpContext.HttpContext ?? throw new NotSupportedException();

        if ( !(context.Items.TryGetValue("AntiforgerytokenValidated", out var isAntiforgerytokenValidated) && ((bool?)isAntiforgerytokenValidated).GetValueOrDefault()))
        {
            throw new UnauthorizedAccessException("Antiforgery token not validated");
        }
        
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
