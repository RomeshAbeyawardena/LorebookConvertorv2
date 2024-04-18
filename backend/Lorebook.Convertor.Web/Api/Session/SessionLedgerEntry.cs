using Lorebook.Convertor.Domain;

namespace Lorebook.Convertor.Web.Api.Session;

public class SessionLedgerEntry : ISessionLedgerEntry
{
    public Guid SessionId { get; init; }
    public required string Key { get; init; }
    public DateTimeOffset? Expiry { get; init; }
}
