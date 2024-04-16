using Lorebook.Convertor.Web.Api.Extensions;
using Lorebook.Convertor.Web.Api.Session;
using Lorebook.Convertor.Web.Api.Session.Get;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Caching.Distributed;

namespace Lorebook.Convertor.Web.Api.AntiforgeryToken.Post;

public class Handler(IMediator mediator, IDistributedCache distributedCache, IAntiforgery antiforgery, IHttpContextAccessor httpContext) : IRequestHandler<Command, SessionData>
{
    public async Task<SessionData> Handle(Command request, CancellationToken cancellationToken)
    {
        var session = await mediator.Send(new Query { SessionId = request.SessionId }, cancellationToken) 
            ?? throw new NullReferenceException("Session not found");

        var tokens = antiforgery.GetTokens(httpContext.HttpContext ?? throw new InvalidOperationException());
        session.AntiforgeryToken = tokens.RequestToken;

        await distributedCache.CommitSessionData(session, cancellationToken);

        return session;
    }
}
