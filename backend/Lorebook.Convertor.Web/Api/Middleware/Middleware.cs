namespace Lorebook.Convertor.Web.Api.Middleware;

public static partial class Middleware
{
    
    public static WebApplication UseOwnMiddleware(this WebApplication builder)
    {
        builder
            .Use(SessionMiddleware.SessionHandler)
            .Use(AntiforgeryTokenMiddleware.AntiforgeryTokenHandler);
        return builder;
    }
}
