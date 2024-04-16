using Lorebook.Convertor.Web.Api.Session;

namespace Lorebook.Convertor.Web.Api.Middleware
{
    public static class AntiforgeryTokenMiddleware
    {
        public static async Task AntiforgeryTokenHandler(HttpContext context, RequestDelegate requestDelegate)
        {
            try
            {
                if (context.Request.Headers.TryGetValue("X-AFT", out var antiForgeryValue))
                {
                    var token = antiForgeryValue.FirstOrDefault();
                    var session = context.Features.Get<SessionData>();
                    var timeProvider = context.RequestServices.GetRequiredService<TimeProvider>();
                    if (!string.IsNullOrWhiteSpace(token) && session != null && session.Expires > timeProvider.GetUtcNow())
                    {
                        if(string.IsNullOrWhiteSpace(session.AntiforgeryToken))
                        {
                            throw new NullReferenceException("Antiforgery token not set");
                        }

                        if (session.AntiforgeryToken != token)
                        {
                            throw new UnauthorizedAccessException("Antiforgery token is not valid");
                        }
                    }
                }

                context.Items.Add("AntiforgerytokenValidated", true);
                await requestDelegate(context);
            }
            catch (Exception ex ) when (ex is UnauthorizedAccessException || ex is NullReferenceException || ex is InvalidOperationException)
            {
                await SessionMiddleware.Fail(context.Response, ex.Message, 401);
            }
        }
    }
}
