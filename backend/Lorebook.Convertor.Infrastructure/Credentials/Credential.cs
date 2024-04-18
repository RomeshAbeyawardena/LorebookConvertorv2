namespace Lorebook.Convertor.Infrastructure.Credentials;

public class Credential
{
    public Guid Id { get; set; }
    public string? Key { get; set; }
    public string? Name { get; set; }
    public DateTimeOffset? ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }
    public DateTimeOffset Created { get; set; }

    public virtual ICollection<CredentialPermission> Permissions { get; set; } = [];
}
