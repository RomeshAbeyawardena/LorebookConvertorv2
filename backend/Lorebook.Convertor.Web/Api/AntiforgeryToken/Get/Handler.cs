using Lorebook.Convertor.Web.Api.Extensions;
using Lorebook.Convertor.Web.Api.Session;
using Lorebook.Convertor.Web.Api.Session.Get;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Caching.Distributed;

namespace Lorebook.Convertor.Web.Api.AntiforgeryToken.Get;

public class Handler(IMediator mediator, IDistributedCache distributedCache, 
    IAntiforgery antiforgery, IHttpContextAccessor httpContext, TimeProvider timeProvider) : IRequestHandler<Command, AntiforgeryTokenSessionData>
{
    public async Task<AntiforgeryTokenSessionData> Handle(Command request, CancellationToken cancellationToken)
    {
        var session = await mediator.Send(new Query { SessionId = request.SessionId }, cancellationToken) 
            ?? throw new NullReferenceException("Session not found");

        var utcNow = timeProvider.GetUtcNow();
        
        if(session.Expires < utcNow)
        {
            throw new UnauthorizedAccessException("Session expired");
        }

        var tokens = antiforgery.GetTokens(httpContext.HttpContext ?? throw new InvalidOperationException());
        session.AntiforgeryToken = tokens.RequestToken;
        utcNow = timeProvider.GetUtcNow();
        session.Modified = utcNow;
        await distributedCache.CommitSessionData(session, cancellationToken);

        var token = AntiforgeryTokenSessionData.From(session);
        token.CookieName = tokens.CookieToken;
        token.HeaderName = token.HeaderName;
        return token;
    }
}
