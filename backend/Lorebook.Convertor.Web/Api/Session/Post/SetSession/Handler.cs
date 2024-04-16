using Lorebook.Convertor.Domain;
using Lorebook.Convertor.Web.Api.Session.Post.GetSession;
using MediatR;
using MessagePack;
using Microsoft.Extensions.Caching.Distributed;

namespace Lorebook.Convertor.Web.Api.Session.Post.SetSession;

public class Handler(IMediator mediator, IDistributedCache distributedCache, 
    ApplicationSettings applicationSettings, TimeProvider timeProvider) : IRequestHandler<Command, SessionData>
{
    internal async Task CommitSessionData(SessionData sessionData, CancellationToken cancellationToken)
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
            sessionData = await mediator.Send(new Query { SessionId = request.SessionId.Value }, cancellationToken) 
                ?? throw new NullReferenceException("Session not found");

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
