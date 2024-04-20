using HSINet.Shared.Transactional;
using Microsoft.EntityFrameworkCore;

namespace Lorebook.Convertor.Infrastructure;

public class CredentialDbContext(DbContextOptions<CredentialDbContext> options) 
    : DbContext(options), IUnitOfWork
{
    public DbSet<AccessTokens.AccessToken> AccessTokens { get; set; }
    public DbSet<Credentials.Credential> Credentials { get; set; }
    public DbSet<Permissions.Permission> Permissions { get; set; }
}
