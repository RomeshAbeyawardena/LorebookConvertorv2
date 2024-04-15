using MediatR;

namespace Lorebook.Convertor.Web.Api.Lorebook.Post.Convert;

public class Handler : IRequestHandler<Command, Response>
{
    public Task<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
