using MediatR;
using MessagePack;
using Microsoft.Extensions.Caching.Distributed;

namespace Lorebook.Convertor.Web.Api.Session.Post.GetSession;

public class Handler(IDistributedCache distributedCache) : IRequestHandler<Query, SessionData?>
{
    public async Task<SessionData?> Handle(Query request, CancellationToken cancellationToken)
    {
        SessionData? sessionData = null;

        ArgumentNullException.ThrowIfNull(request.SessionId, nameof(request.SessionId));

        var sessionRawData = await distributedCache.GetAsync($"session:{request.SessionId:x}", cancellationToken);
        if (sessionRawData != null)
        {
            using var memoryStream = new MemoryStream(sessionRawData);
            await MessagePackSerializer.SerializeAsync(memoryStream, sessionData, cancellationToken: cancellationToken);
        }

        return sessionData;
    }
}
