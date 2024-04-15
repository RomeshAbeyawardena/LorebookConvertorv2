using System.Reflection;

namespace Lorebook.Convertor.Web.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        return services
            .AddHttpContextAccessor()
            .AddMediatR(cfg => cfg
                .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}
