using Lorebook.Convertor.Domain;
using Lorebook.Convertor.Web.Api.Session.Get;
using MediatR;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Lorebook.Convertor.Web.Api.Middleware;

public static class SessionMiddleware
{
    public static async Task Fail(HttpResponse response, string? reason, int statusCode = 401)
    {
        response.StatusCode = statusCode;
        await response.WriteAsJsonAsync(Result.Error<string>($"Authorisation failed: {reason}", statusCode));
    }

    public static async Task SessionHandler(HttpContext context, RequestDelegate requestDelegate)
    {
        var response = context.Response;
        try
        {
            if (context.Request.Headers.TryGetValue("Session-Key", out var sessionKey))
            {
                var key = sessionKey.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(key))
                {
                    var jsonWebTokenHandler = new JsonWebTokenHandler();
                    var applicationSettings = context.RequestServices.GetRequiredService<ApplicationSettings>();

                    var tokenValidationResult = await jsonWebTokenHandler
                        .ValidateTokenAsync(key, new TokenValidationParameters
                        {
                            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(applicationSettings.TokenKey)),
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidAudiences = applicationSettings.Audiences,
                            ValidIssuers = applicationSettings.Issuers,
                            RequireExpirationTime = true,
                            TokenDecryptionKey = new SymmetricSecurityKey(Convert.FromBase64String(applicationSettings.TokenKey))
                        });

                    if (tokenValidationResult.IsValid)
                    {
                        if (tokenValidationResult.Claims.TryGetValue("Session-Id", out var sessionId))
                        {
                            var mediator = context.RequestServices.GetRequiredService<IMediator>();
                            var timeProvider = context.RequestServices.GetRequiredService<TimeProvider>();
                            var session = await mediator.Send(new Query { SessionId = (Guid)sessionId });
                            if (session == null || session.Expires < timeProvider.GetUtcNow())
                            {
                                throw new UnauthorizedAccessException("Session expired or invalid");
                            }

                            context.Features.Set(session);
                        }
                    }
                    else
                        throw new UnauthorizedAccessException("Session expired or invalid");
                }
            }

            await requestDelegate(context);
        }
        catch (UnauthorizedAccessException exception)
        {
            await Fail(response, exception.Message);
        }
    }

}
