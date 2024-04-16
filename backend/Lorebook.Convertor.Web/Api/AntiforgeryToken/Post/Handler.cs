using Lorebook.Convertor.Web.Api.Session;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Lorebook.Convertor.Web.Api.AntiforgeryToken.Post;

public class Handler(IDistributedCache distributedCache) : IRequestHandler<Command, SessionData>
{
    public Task<SessionData> Handle(Command request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();

    }
}
