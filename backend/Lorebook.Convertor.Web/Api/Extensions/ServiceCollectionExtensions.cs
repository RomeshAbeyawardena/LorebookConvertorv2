using Lorebook.Convertor.Domain;
using Lorebook.Convertor.Web.Api.AntiforgeryToken.Extensions;
using System.Reflection;

namespace Lorebook.Convertor.Web.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        return services
            .AddSingleton(TimeProvider.System)
            .AddSingleton<ApplicationSettings>()
            .AddHttpContextAccessor()
            .AddDistributedMemoryCache()
            .AddAntiForgeryTokenServices()
            .AddMediatR(cfg => cfg
                .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}
