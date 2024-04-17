using Lorebook.Convertor.Domain;

namespace Lorebook.Convertor.Web.Api.Session;

[MessagePack.MessagePackObject(true)]
public class SessionData : ISessionData
{
    public Guid SessionId { get; set; }
    public string? AntiforgeryToken { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }
    public DateTimeOffset? Expires { get; set; }
    public string? WebToken { get; set; }
}
