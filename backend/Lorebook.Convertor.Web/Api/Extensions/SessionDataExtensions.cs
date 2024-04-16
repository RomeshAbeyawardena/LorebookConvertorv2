using Lorebook.Convertor.Web.Api.Session;

namespace Lorebook.Convertor.Web.Api.Extensions;

public static class SessionDataExtensions
{
    public static bool IsValid(this SessionData? sessionData, TimeProvider? timeProvider = null)
    {
        timeProvider ??= TimeProvider.System;

        return sessionData != null 
            && sessionData.SessionId != Guid.Empty
            && sessionData.Expires > timeProvider.GetUtcNow();
    }
}
