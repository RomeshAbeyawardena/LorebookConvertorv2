namespace Lorebook.Convertor.Domain.Exceptions;

public class ConfigurationMissingException(string parameterName)
    : NullReferenceException($"Configuration missing: {parameterName}")
{
    public string ParameterName { get; } = parameterName;
}
