using Lorebook.Convertor.Web.Api.Session;
using MessagePack;
using Microsoft.Extensions.Caching.Distributed;

namespace Lorebook.Convertor.Web.Api.Extensions;

public static class DistributedCacheExtensions
{
    public static async Task CommitSessionData(this IDistributedCache distributedCache, SessionData sessionData, CancellationToken cancellationToken)
    {
        var serialisedSessionData = MessagePackSerializer.Serialize(sessionData, cancellationToken: cancellationToken);
        await distributedCache.SetAsync($"session:{sessionData.SessionId:x}", serialisedSessionData, cancellationToken);
    }
}
