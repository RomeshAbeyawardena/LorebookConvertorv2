namespace Lorebook.Convertor.Web.Api.Middleware
{
    public static class AntiforgeryTokenMiddleware
    {
        public static async Task AntiforgeryTokenHandler(HttpContext context, RequestDelegate requestDelegate)
        {

            await requestDelegate(context);
        }
    }
}
