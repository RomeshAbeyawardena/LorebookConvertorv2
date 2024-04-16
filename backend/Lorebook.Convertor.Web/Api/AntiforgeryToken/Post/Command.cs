using Lorebook.Convertor.Web.Api.Session;
using MediatR;

namespace Lorebook.Convertor.Web.Api.AntiforgeryToken.Post;

public record Command : IRequest<SessionData>
{
    public Guid? SessionId { get; set; }
}
