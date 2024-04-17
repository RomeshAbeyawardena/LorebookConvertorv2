using Lorebook.Convertor.Domain.Exceptions;
using Lorebook.Convertor.Web.Api.Extensions;
using Lorebook.Convertor.Web.Api.Session;

namespace Lorebook.Convertor.Web.Api.Middleware
{
    public static class AntiforgeryTokenMiddleware
    {
        public static async Task AntiforgeryTokenHandler(HttpContext context, RequestDelegate requestDelegate)
        {
            var timeProvider = context.RequestServices.GetRequiredService<TimeProvider>();

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
                    }
                }

                context.Items.Add("AntiforgerytokenValidated", true);
                await requestDelegate(context);
            }
            catch (System.Exception ex ) when (ex is UnauthorizedAccessException 
                || ex is NullReferenceException 
                || ex is InvalidOperationException)
            {
                await SessionMiddleware.Fail(timeProvider, context.Response, ex.Message, 401);
            }
        }
    }
}
