using MediatR;

namespace Lorebook.Convertor.Web.Api.Session.Post.SetSession;

public class Command : IRequest<SessionData>
{
    /// <summary>
    /// Gets or sets the session Id to renew if blank, creates a new session
    /// </summary>
    public Guid? SessionId { get; set; }
}
