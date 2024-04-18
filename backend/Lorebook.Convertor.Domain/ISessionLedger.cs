namespace Lorebook.Convertor.Domain;

public interface ISessionLedgerEntry
{
    Guid SessionId { get; set; }
    string Key { get; set; }
    DateTimeOffset Expiry { get; set; }
}

public interface ISessionLedger : IEnumerable<ISessionLedgerEntry>
{
    ISessionLedger Add(ISessionLedgerEntry entry);
    Task RemoveExpired(IDistributedCache distributedCache);
}