namespace Lorebook.Convertor.Domain.Exceptions;

public class EntityNotFoundException(Type entityType) 
    : NullReferenceException($"Entity of type {entityType.Name} not found"), IStatusCodeException
{
    public int StatusCode { get; } = 404;
}
