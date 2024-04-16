using Lorebook.Convertor.Domain;
using Lorebook.Convertor.Web.Api.Extensions;
using Lorebook.Convertor.Web.Api.Session.Get;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Lorebook.Convertor.Web.Api.Session.Post.SetSession;

public class Handler(IMediator mediator, IDistributedCache distributedCache, 
    ApplicationSettings applicationSettings, TimeProvider timeProvider) : IRequestHandler<Command, SessionData>
{
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
                await distributedCache.CommitSessionData(sessionData, cancellationToken);
            }

            return sessionData;
        }

        utcNow = timeProvider.GetUtcNow();
        sessionData.SessionId = Guid.NewGuid();
        sessionData.Expires = utcNow.AddMinutes(applicationSettings.SessionValidityPeriodInMinutes);
        sessionData.Created = utcNow;

        await distributedCache.CommitSessionData(sessionData, cancellationToken);
        return sessionData;
    }
}
