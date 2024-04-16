using MediatR;

namespace Lorebook.Convertor.Web.Api.Session.Get;

public record Query : IRequest<SessionData?>
{
    public Guid? SessionId { get; init; }
}
