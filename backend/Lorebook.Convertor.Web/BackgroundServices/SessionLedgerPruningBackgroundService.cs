using Lorebook.Convertor.Domain;
using Microsoft.Extensions.Caching.Distributed;

namespace Lorebook.Convertor.Web.BackgroundServices;

public class SessionLedgerPruningBackgroundService(ISessionLedger sessionLedger, 
    IDistributedCache distributedCache, ApplicationSettings applicationSettings) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if(await sessionLedger.HasExpiredEntries(stoppingToken))
            {
                await sessionLedger.RemoveExpired(distributedCache, stoppingToken);
            }

            await Task.Delay(applicationSettings.BackgroundServiceTimeoutInterval, stoppingToken);
        }
    }
}
