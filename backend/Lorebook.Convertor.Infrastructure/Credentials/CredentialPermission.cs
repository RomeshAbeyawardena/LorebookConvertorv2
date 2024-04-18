namespace Lorebook.Convertor.Infrastructure.Credentials;

public class CredentialPermission
{
    public Guid Id { get; set; }
    public Guid CredentialId { get; set; }
    public Guid PermissionId { get; set; }
    public DateTimeOffset? ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }
    public virtual Credential? Credential { get; set; }
}
