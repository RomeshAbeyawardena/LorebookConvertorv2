namespace Lorebook.Convertor.Web.Api;

public class Result
{
    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(200, null, value);
    }

    public static Result<T> Error<T>(string message, int statusCode = 400)
    {
        return new Result<T>(statusCode, message, default);
    }

}

public class Result<T>(int code, string? error = null, T? data = default)
{
    public int Code { get; } = code;
    public string? Error { get; } = error;
    public T? Data { get; } = data;
}
