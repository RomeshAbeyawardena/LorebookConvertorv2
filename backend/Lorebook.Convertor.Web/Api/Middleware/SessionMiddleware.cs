using Lorebook.Convertor.Domain;
using Lorebook.Convertor.Domain.Exceptions;
using Lorebook.Convertor.Web.Api.Extensions;
using Lorebook.Convertor.Web.Api.Session.Get;
using MediatR;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Lorebook.Convertor.Web.Api.Middleware;

public class SessionMiddlewareInstance
{

}

public static class SessionMiddleware
{
    public static async Task Fail(TimeProvider timeProvider, HttpResponse response, 
        string? reason, int statusCode = 401)
    {
        response.StatusCode = statusCode;
        await response.WriteAsJsonAsync(Result.Error<string>(timeProvider, 
            $"Authorisation failed: {reason}", statusCode));
    }

    public static async Task SessionHandler(HttpContext context, RequestDelegate requestDelegate)
    {
        var requestServices = context.RequestServices;
        var logger = requestServices.GetRequiredService<ILogger<SessionMiddlewareInstance>>();
        var timeProvider = requestServices.GetRequiredService<TimeProvider>();
        var response = context.Response;
        try
        {
            if (context.Request.Headers.TryGetValue("Session-Key", out var sessionKey))
            {
                var key = sessionKey.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(key))
                {
                    var jsonWebTokenHandler = new JsonWebTokenHandler();
                    var applicationSettings = requestServices
                        .GetRequiredService<ApplicationSettings>();
                    
                    var tokenValidationResult = await jsonWebTokenHandler
                        .ValidateTokenAsync(key, new TokenValidationParameters
                        {
                            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(applicationSettings.TokenKey 
                                ?? throw new ConfigurationMissingException(
                                    nameof(applicationSettings.TokenKey)))),
                            RequireSignedTokens = true,
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidAudiences = applicationSettings.Audiences,
                            ValidIssuers = applicationSettings.Issuers,
                            RequireExpirationTime = true,
                            TokenDecryptionKey = new SymmetricSecurityKey(Convert.FromBase64String(applicationSettings.EncryptionKey
                                ?? throw new ConfigurationMissingException(nameof(applicationSettings.EncryptionKey)))),
                        });

                    if (tokenValidationResult.IsValid)
                    {
                        if (tokenValidationResult.Claims.TryGetValue("Session-Id", out var sessionId))
                        {
                            var mediator = context.RequestServices.GetRequiredService<IMediator>();
                            if(!Guid.TryParse(sessionId?.ToString(), out var id))
                            {
                                throw new InvalidClaimsException("Invalid claims");
                            }

                            var session = await mediator.Send(new Query { SessionId = id });
                            if (!session.IsValid(timeProvider))
                            {
                                throw new InvalidOrExpiredSessionException();
                            }

                            context.Features.Set(session);
                        }
                    }
                    else
                        throw new InvalidOrExpiredSessionException();
                }
            }

            await requestDelegate(context);
        }
        catch (System.Exception exception) 
            when (exception is IStatusCodeException statusCodeException)
        {
            logger.LogError(exception, "Session middleware failed to execute: {message}", 
                exception.Message);
            await Fail(timeProvider, response, exception.Message, statusCodeException.StatusCode);
        }
    }
}
