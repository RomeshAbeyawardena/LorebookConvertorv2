namespace Lorebook.Convertor.Domain;

public interface ISessionData
{
    Guid SessionId { get; set; }
    string? AntiforgeryToken { get; set; }
    DateTimeOffset Created { get; set; }
    DateTimeOffset? Modified { get; set; }
    DateTimeOffset? Expires { get; set; }
    string? WebToken { get; set; }
}
