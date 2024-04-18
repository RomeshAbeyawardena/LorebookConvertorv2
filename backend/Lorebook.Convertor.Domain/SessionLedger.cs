using Microsoft.Extensions.Caching.Distributed;
using System.Collections;
using System.Collections.Concurrent;

namespace Lorebook.Convertor.Domain;

public class SessionLedger(TimeProvider timeProvider) : ISessionLedger
{
    private readonly ConcurrentDictionary<string, ISessionLedgerEntry> ledgerEntries = [];
    public ISessionLedger Add(ISessionLedgerEntry entry)
    {
        ledgerEntries.TryAdd(entry.Key, entry);
        return this;
    }

    public IEnumerator<ISessionLedgerEntry> GetEnumerator()
    {
        return ledgerEntries.Values.GetEnumerator();
    }

    public ValueTask<bool> HasExpiredEntries(CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(this.Any(l => l.Expiry < timeProvider.GetUtcNow()));
    }

    public async Task RemoveExpired(IDistributedCache distributedCache, CancellationToken cancellationToken)
    {
        Queue<ISessionLedgerEntry> removalQueue = new (this
            .Where(l => l.Expiry < timeProvider.GetUtcNow()));

        while (removalQueue.TryDequeue(out var sessionLedgerEntry))
        {
            await distributedCache.RemoveAsync(sessionLedgerEntry.Key, cancellationToken);
            ledgerEntries.TryRemove(sessionLedgerEntry.Key, out _);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}