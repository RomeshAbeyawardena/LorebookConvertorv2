namespace Lorebook.Convertor.Web.Api.AntiforgeryToken.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAntiForgeryTokenServices(this IServiceCollection services)
    {
        return services.AddAntiforgery();
    }
}
