namespace Lorebook.Convertor.Web.Api.Extensions;

public static class HttpContextExtensions
{
    public static bool IsAntiForgeryTokenValidated(this HttpContext context)
    {
        return context.Items.TryGetValue("AntiForgeryTokenValidated", out var isAntiforgerytokenValidated)
            && ((bool?)isAntiforgerytokenValidated).GetValueOrDefault();
    }
}
