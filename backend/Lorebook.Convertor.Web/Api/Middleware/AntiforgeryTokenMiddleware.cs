using Lorebook.Convertor.Domain;
using Lorebook.Convertor.Domain.Exceptions;
using Lorebook.Convertor.Web.Api.Extensions;
using Lorebook.Convertor.Web.Api.Session;
using Microsoft.Extensions.Caching.Distributed;

namespace Lorebook.Convertor.Web.Api.Middleware
{
    public class AntiforgeryTokenMiddlewareInstance { };

    public static class AntiforgeryTokenMiddleware
    {
        public static async Task AntiforgeryTokenHandler(HttpContext context, RequestDelegate requestDelegate)
        {
            var requestServices = context.RequestServices;
            var timeProvider = requestServices.GetRequiredService<TimeProvider>();
            var logger = requestServices.GetRequiredService<ILogger<AntiforgeryTokenMiddlewareInstance>>();
            try
            {
                if (context.Request.Headers.TryGetValue("X-AFT", out var antiForgeryValue))
                {
                    var token = antiForgeryValue.FirstOrDefault();
                    var session = context.Features.Get<SessionData>();
                    
                    if (!string.IsNullOrWhiteSpace(token) && session.IsValid(timeProvider))
                    {
                        if(string.IsNullOrWhiteSpace(session!.AntiforgeryToken))
                        {
                            throw new EntityNotFoundException();
                        }

                        if (session.AntiforgeryToken != token)
                        {
                            throw new InvalidOrExpiredAntiForgeryException();
                        }

                        var distributedCache = requestServices
                            .GetRequiredService<IDistributedCache>();

                        session.AntiforgeryToken = null;

                        await distributedCache.CommitSessionData(null,
                            session, CancellationToken.None);

                        context.Items.Add("AntiForgeryTokenValidated", true);
                    }
                }

                await requestDelegate(context);
            }
            catch (System.Exception exception)
                when (exception is IStatusCodeException statusCodeException)
            {
                logger.LogError(exception, "Session middleware failed to execute: {message}", 
                    exception.Message);
                await SessionMiddleware
                    .Fail(timeProvider, context.Response, exception.Message, 
                        statusCodeException.StatusCode);
            }
        }
    }
}
