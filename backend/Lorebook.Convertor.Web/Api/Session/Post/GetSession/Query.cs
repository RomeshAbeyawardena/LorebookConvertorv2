using MediatR;

namespace Lorebook.Convertor.Web.Api.Session.Post.GetSession;

public record Query : IRequest<SessionData?>
{
    public Guid? SessionId { get; init; }
}
