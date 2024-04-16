using Lorebook.Convertor.Domain;
using MediatR;
using MessagePack;
using Microsoft.Extensions.Caching.Distributed;

namespace Lorebook.Convertor.Web.Api.Session.Post.SetSession;

public class Handler(IDistributedCache distributedCache, ApplicationSettings applicationSettings, TimeProvider timeProvider) : IRequestHandler<Command, SessionData>
{
    private async Task CommitSessionData(SessionData sessionData, CancellationToken cancellationToken)
    {
        var serialisedSessionData = MessagePackSerializer.Serialize(sessionData, cancellationToken: cancellationToken);
        await distributedCache.SetAsync($"session:{sessionData.SessionId:x}", serialisedSessionData, cancellationToken);
    }

    public async Task<SessionData> Handle(Command request, CancellationToken cancellationToken)
    {
        SessionData sessionData = new();
        DateTimeOffset utcNow;
        if (request.SessionId.HasValue)
        {
            var sessionRawData = await distributedCache.GetAsync($"session:{request.SessionId:x}", cancellationToken);
            if (sessionRawData != null)
            {
                using var memoryStream = new MemoryStream(sessionRawData);
                await MessagePackSerializer.SerializeAsync(memoryStream, sessionData, cancellationToken: cancellationToken);
            }

            utcNow = timeProvider.GetUtcNow();
            if (sessionData.Expires < utcNow)
            {
                sessionData.Modified = utcNow;
                sessionData.Expires = utcNow.AddMinutes(applicationSettings.SessionValidityPeriodInMinutes);
                await CommitSessionData(sessionData, cancellationToken);
            }

            return sessionData;
        }

        utcNow = timeProvider.GetUtcNow();
        sessionData.SessionId = Guid.NewGuid();
        sessionData.Expires = utcNow.AddMinutes(applicationSettings.SessionValidityPeriodInMinutes);
        sessionData.Created = utcNow;

        await CommitSessionData(sessionData, cancellationToken);
        return sessionData;
    }
}
