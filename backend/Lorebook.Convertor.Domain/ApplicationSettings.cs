using Microsoft.Extensions.Configuration;

namespace Lorebook.Convertor.Domain;

public record ApplicationSettings
{
    public ApplicationSettings(IConfiguration configuration)
    {
        configuration.Bind(this);
    }

    public int BackgroundServiceTimeoutInterval { get; set; }
    public int SessionValidityPeriodInMinutes { get; set; }
    public IEnumerable<string> Audiences { get; set; } = [];
    public IEnumerable<string> Issuers { get; set; } = [];
    public string? TokenKey { get; set; }
    public string? EncryptionKey { get; set; }
}
