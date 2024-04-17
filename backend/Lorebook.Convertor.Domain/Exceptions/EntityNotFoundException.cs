namespace Lorebook.Convertor.Domain.Exceptions;

public class EntityNotFoundException(Type? entityType = null) 
    : NullReferenceException(entityType == null 
        ? "Entity not found"
        : $"Entity of type {entityType?.Name} not found"), IStatusCodeException
{
    public int StatusCode { get; } = 404;
}
