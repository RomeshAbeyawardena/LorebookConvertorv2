namespace Lorebook.Convertor.Infrastructure.AccessTokens;

public class AccessToken
{
    public Guid Id { get; set; }
    public Guid CredentialId { get; set; }
    public string? Key { get; set; }
    public bool ImpersonateCredentialAccess { get; set; }
    public DateTimeOffset? ValidFrom { get; set; }
    public DateTimeOffset? ValidTo { get; set; }
    public virtual Credentials.Credential? Credential { get; set; }
}
