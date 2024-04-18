namespace Lorebook.Convertor.Domain.Exceptions;

public class InvalidClaimsException(string? expectationMessage)
    : InvalidCastException(string.IsNullOrWhiteSpace(expectationMessage)
        ? "Invalid claims"
        : $"Invalid claims: {expectationMessage}"), IStatusCodeException
{
    public int StatusCode { get; } = 400;
}
