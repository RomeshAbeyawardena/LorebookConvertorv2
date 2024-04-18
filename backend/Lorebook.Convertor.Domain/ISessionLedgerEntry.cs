namespace Lorebook.Convertor.Domain;

public interface ISessionLedgerEntry
{
    Guid SessionId { get; }
    string Key { get; }
    DateTimeOffset? Expiry { get; }
}
