namespace Restaurants.Domain.Exceptions;

/// <summary>
/// Rpresents an exception that is thrown when Entity not found
/// </summary>
/// <param name="message"></param>
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityType, string entityId)
        : base($"{entityType} {entityId} doesn't exist") { }
}
