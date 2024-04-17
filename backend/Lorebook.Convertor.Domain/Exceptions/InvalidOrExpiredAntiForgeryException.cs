namespace Lorebook.Convertor.Domain.Exceptions;

public class InvalidOrExpiredAntiForgeryException() : 
    UnauthorizedAccessException("Invalid or expired Anti-forgery token"), IStatusCodeException
{
    public int StatusCode => 403;
}
