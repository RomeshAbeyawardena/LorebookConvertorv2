using Lorebook.Convertor.Domain;
using Lorebook.Convertor.Web.Api.Session;
using MessagePack;
using Microsoft.Extensions.Caching.Distributed;

namespace Lorebook.Convertor.Web.Api.Extensions;

public static class DistributedCacheExtensions
{
    public static async Task CommitSessionData(this IDistributedCache distributedCache, 
        ISessionLedger? sessionLedger,
        SessionData sessionData, CancellationToken cancellationToken)
    {
        var serialisedSessionData = MessagePackSerializer.Serialize(sessionData, cancellationToken: cancellationToken);
        var sessionKey = $"session:{sessionData.SessionId:x}";
        await distributedCache.SetAsync(sessionKey, serialisedSessionData, cancellationToken);

        sessionLedger?.Add(new SessionLedgerEntry {
            Expiry = sessionData.Expires,
            Key = sessionKey,
            SessionId = sessionData.SessionId });
    }
}
