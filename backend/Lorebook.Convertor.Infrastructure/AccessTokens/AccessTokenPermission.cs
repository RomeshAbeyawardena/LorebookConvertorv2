using HSINet.Shared.EntityAttributes;

namespace Lorebook.Convertor.Infrastructure.AccessTokens;

public class AccessTokenPermission : IIdentity, ICreated
{
    public Guid? Id { get; set; }
    public Guid AccessTokenId { get; set; }
    public Guid PermissionId { get; set; }
    public virtual AccessToken? AccessToken { get; set; }
    public virtual Permissions.Permission? Permission { get; set; }
    public DateTimeOffset Created { get; set; }
}
