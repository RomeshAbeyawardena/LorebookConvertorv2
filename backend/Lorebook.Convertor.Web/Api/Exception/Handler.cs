using Microsoft.AspNetCore.Diagnostics;

namespace Lorebook.Convertor.Web.Api.Exception;

public class Handler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, System.Exception exception, CancellationToken cancellationToken)
    {
        var exceptionCollection = httpContext.RequestServices.GetRequiredService<TypeCollection>();
        var response = httpContext.Response;
        await response.WriteAsJsonAsync(Result.Error<bool>(exception.Message), cancellationToken);
#if DEBUG
        return false;
#else
        return exceptionCollection.Contains(exception);
#endif
    }
}
