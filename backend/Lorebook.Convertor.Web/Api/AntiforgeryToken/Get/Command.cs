using MediatR;

namespace Lorebook.Convertor.Web.Api.AntiforgeryToken.Get;

public record Command : IRequest<AntiforgeryTokenSessionData>
{
    public Guid? SessionId { get; set; }
}
