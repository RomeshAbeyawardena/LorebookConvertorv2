using Microsoft.Extensions.Caching.Distributed;
using System.Collections;
using System.Collections.Concurrent;

namespace Lorebook.Convertor.Domain;

public class SessionLedger(TimeProvider timeProvider) : ISessionLedger
{
    private readonly ConcurrentBag<ISessionLedgerEntry> ledgerEntries = [];
    public ISessionLedger Add(ISessionLedgerEntry entry)
    {
        ledgerEntries.Add(entry);
        return this;
    }

    public IEnumerator<ISessionLedgerEntry> GetEnumerator()
    {
        return ledgerEntries.GetEnumerator();
    }

    public ValueTask<bool> HasExpiredEntries(CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(ledgerEntries.Any(l => l.Expiry < timeProvider.GetUtcNow()));
    }

    public async Task RemoveExpired(IDistributedCache distributedCache, CancellationToken cancellationToken)
    {
        var removalQueue = new Queue<ISessionLedgerEntry>(ledgerEntries
            .Where(l => l.Expiry < timeProvider.GetUtcNow()));

        while (removalQueue.TryDequeue(out var sessionLedgerEntry))
        {
            await distributedCache.RemoveAsync(sessionLedgerEntry.Key, cancellationToken);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ledgerEntries.GetEnumerator();
    }
}