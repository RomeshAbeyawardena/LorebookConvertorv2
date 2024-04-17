using Lorebook.Convertor.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Lorebook.Convertor.Web.Api.Exception;

public class Handler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, System.Exception exception, CancellationToken cancellationToken)
    {
        var exceptionCollection = httpContext.RequestServices.GetRequiredService<TypeCollection>();
        var timeProvider = httpContext.RequestServices.GetRequiredService<TimeProvider>();
        var response = httpContext.Response;
        var statusCode = (exception is IStatusCodeException statusCodeException)
            ? statusCodeException.StatusCode
            : 500;

        await response.WriteAsJsonAsync(Result.Error<bool>(timeProvider, exception.Message, statusCode), 
            cancellationToken);
#if DEBUG
        return false;
#else
        return statusCode != 500 || exceptionCollection.Contains(exception.GetType());
#endif
    }
}
