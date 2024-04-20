﻿using Microsoft.Extensions.Caching.Distributed;

namespace Lorebook.Convertor.Domain;

public interface ISessionLedger : IEnumerable<ISessionLedgerEntry>
{
    ISessionLedger Add(ISessionLedgerEntry entry);
    Task<int> RemoveExpired(IDistributedCache distributedCache, CancellationToken cancellationToken);
    ValueTask<bool> HasExpiredEntries(CancellationToken cancellationToken);
}