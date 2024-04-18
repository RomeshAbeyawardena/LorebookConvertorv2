namespace Lorebook.Convertor.Infrastructure.AccessTokens;

public interface IRepository<T>
    where T: class
{
    Task<T> Find(CancellationToken cancellationToken, params object[] keys);
    Task<T> Upsert(AccessToken accessToken, CancellationToken cancellationToken);
}

public interface IAccessTokenRepository : IRepository<AccessToken>
{
    Task<AccessToken> GetAccessToken(string key, CancellationToken cancellationToken);
    /// <summary>
    /// Gets a list of access tokens for a credential
    /// </summary>
    /// <param name="credentialId">The credential ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<AccessToken>> GetAccessTokensAsync(Guid credentialId, 
        CancellationToken cancellationToken);
}
