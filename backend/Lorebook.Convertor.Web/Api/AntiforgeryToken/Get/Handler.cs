using Lorebook.Convertor.Domain;
using Lorebook.Convertor.Domain.Exceptions;
using Lorebook.Convertor.Web.Api.Extensions;
using Lorebook.Convertor.Web.Api.Session;
using Lorebook.Convertor.Web.Api.Session.Get;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Caching.Distributed;

namespace Lorebook.Convertor.Web.Api.AntiforgeryToken.Get;

public class Handler(IMediator mediator, IDistributedCache distributedCache, ISessionLedger sessionLedger, 
    IAntiforgery antiforgery, IHttpContextAccessor httpContext, TimeProvider timeProvider) : IRequestHandler<Command, AntiforgeryTokenSessionData>
{
    public async Task<AntiforgeryTokenSessionData> Handle(Command request, CancellationToken cancellationToken)
    {
        var session = await mediator.Send(new Query { SessionId = request.SessionId }, cancellationToken) 
            ?? throw new InvalidOrExpiredSessionException();

        if(!session.IsValid(timeProvider))
        {
            throw new InvalidOrExpiredSessionException();
        }

        var tokens = antiforgery.GetTokens(httpContext.HttpContext ?? throw new InvalidOperationException());
        session.AntiforgeryToken = tokens.RequestToken;
        var utcNow = timeProvider.GetUtcNow();
        session.Modified = utcNow;
        await distributedCache.CommitSessionData(sessionLedger, session, cancellationToken);

        var token = AntiforgeryTokenSessionData.From(session);
        token.CookieName = tokens.CookieToken;
        token.HeaderName = token.HeaderName;
        return token;
    }
}
