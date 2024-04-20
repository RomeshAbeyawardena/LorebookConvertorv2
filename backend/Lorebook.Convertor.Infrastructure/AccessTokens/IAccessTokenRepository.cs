using HSINet.Shared.Infrastructure;
using HSINet.Shared.Infrastructure.Interceptors.Providers;
using HSINet.Shared.Transactional;
using HSINet.Shared.TypeCache;
using System.Data.Entity;

namespace Lorebook.Convertor.Infrastructure.AccessTokens;

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

public class AccessTokenRepository(CredentialDbContext context,
    IRepositoryInterceptorFactoryProvider repositoryInterceptorFactoryProvider,
    ITypeCacheProvider typeCacheProvider)
    : RepositoryBase<AccessToken, CredentialDbContext>(context,
        repositoryInterceptorFactoryProvider, typeCacheProvider),
            IAccessTokenRepository
{
    public async Task<AccessToken> GetAccessToken(string key, CancellationToken cancellationToken)
    {
        return await Context.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Key == key, cancellationToken);
    }

    public async Task<IEnumerable<AccessToken>> GetAccessTokensAsync(Guid credentialId, CancellationToken cancellationToken)
    {
        return await Context.AsNoTracking()
            .Where(c => c.CredentialId == credentialId)
            .Take(100)
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }
}