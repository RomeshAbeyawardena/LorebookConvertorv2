using HSINet.Shared.EntityAttributes;

namespace Lorebook.Convertor.Infrastructure.AccessTokens;

public class AccessToken : IIdentity, ICreated
{
    /// <summary>
    /// Gets or sets the unique identifier of the access token.
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the credential associated with the access token.
    /// </summary>
    public Guid CredentialId { get; set; }

    /// <summary>
    /// Gets or sets the key of the access token.
    /// </summary>
    public string? Key { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the access token should impersonate credential permissions.
    /// </summary>
    public bool ImpersonateCredentialPermissions { get; set; }

    /// <summary>
    /// Gets or sets the valid from date and time of the access token.
    /// </summary>
    public DateTimeOffset? ValidFrom { get; set; }

    /// <summary>
    /// Gets or sets the valid to date and time of the access token.
    /// </summary>
    public DateTimeOffset? ValidTo { get; set; }

    /// <summary>
    /// Gets or sets the credential associated with the access token.
    /// </summary>
    public virtual Credentials.Credential? Credential { get; set; }

    public virtual ICollection<AccessTokenPermission> Permissions { get; set; } = [];
    
    public DateTimeOffset Created { get; set; }
}
