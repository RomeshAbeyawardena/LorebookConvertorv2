using Lorebook.Convertor.Web.Api.Session;

namespace Lorebook.Convertor.Web.Api.AntiforgeryToken;

public class AntiforgeryTokenSessionData : SessionData
{
    public string? CookieName { get; set; }
    public string? HeaderName { get; set; }
    public static AntiforgeryTokenSessionData From(SessionData sessionData)
    {
        //map AntiforgeryTokenSessionData  from session data  model
        return new AntiforgeryTokenSessionData
        {
            AntiforgeryToken = sessionData.AntiforgeryToken,
            Created = sessionData.Created,
            Expires = sessionData.Expires,
            Modified = sessionData.Modified,
            SessionId = sessionData.SessionId
        };
    }
}
