namespace Lorebook.Convertor.Domain.Exceptions;

public class InvalidOrExpiredSessionException()
    : UnauthorizedAccessException("Invalid or expired session"), IStatusCodeException
{
    public ISessionData? SessionData { get; set; }
    public int StatusCode => 403;
}
