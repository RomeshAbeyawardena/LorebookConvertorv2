using MediatR;

namespace Lorebook.Convertor.Web.Api.Lorebook.Post.Convert;

public record Command : IRequest<Response>
{
    public required FormFile File { get; init; }
    public required string Version { get; init; }
}
