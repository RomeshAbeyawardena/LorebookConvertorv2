using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

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
    : IStatusCodeActionResult, IActionResult
{
    public int Code { get; } = code;
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Error { get; } = error;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; } = data;

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public int? StatusCode => Code;

    public Task ExecuteResultAsync(ActionContext context)
    {
        return context.HttpContext.Response.WriteAsJsonAsync(this);
    }
}
