using Microsoft.Extensions.Configuration;

namespace Lorebook.Convertor.Domain;

public record ApplicationSettings
{
    public ApplicationSettings(IConfiguration configuration)
    {
        configuration.Bind(this);
    }

    public int SessionValidityPeriodInMinutes { get; set; }
}
