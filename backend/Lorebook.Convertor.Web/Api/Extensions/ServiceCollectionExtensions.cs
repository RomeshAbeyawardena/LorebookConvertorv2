using Lorebook.Convertor.Domain;
using Lorebook.Convertor.Domain.Exceptions;
using Lorebook.Convertor.Web.Api.AntiforgeryToken.Extensions;
using Lorebook.Convertor.Web.Api.Exception;
using System.Reflection;

namespace Lorebook.Convertor.Web.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHandledExceptions(this IServiceCollection services,
        Action<TypeCollectionBuilder> buildAction)
    {
        var builder = new TypeCollectionBuilder();

        buildAction(builder);
        return services.AddSingleton(builder.Build());
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        return services
            .AddHandledExceptions(b => b.AddRange(
                typeof(EntityNotFoundException),
                typeof(UnauthorizedAccessException)))
            .AddSingleton(TimeProvider.System)
            .AddSingleton<ApplicationSettings>()
            .AddHttpContextAccessor()
            .AddDistributedMemoryCache()
            .AddAntiForgeryTokenServices()
            .AddExceptionHandler<Handler>()
            .AddProblemDetails()
            .AddMediatR(cfg => cfg
                .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}
