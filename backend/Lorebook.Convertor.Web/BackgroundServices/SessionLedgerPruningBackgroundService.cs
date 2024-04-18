using Lorebook.Convertor.Domain;
using Microsoft.Extensions.Caching.Distributed;

namespace Lorebook.Convertor.Web.BackgroundServices;

public class SessionLedgerPruningBackgroundService(ISessionLedger sessionLedger,
    ILogger<SessionLedgerPruningBackgroundService> logger,
    IDistributedCache distributedCache, ApplicationSettings applicationSettings) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Session ledger pruning background service starting");
        while (!stoppingToken.IsCancellationRequested)
        {
            if(await sessionLedger.HasExpiredEntries(stoppingToken))
            {
                logger.LogInformation("Has expired entries");
                
                var removedItemsCount = await sessionLedger
                    .RemoveExpired(distributedCache, stoppingToken);

                logger.LogInformation("Removed {removedItemsCount} expired entries prunes", 
                    removedItemsCount);
            }

            await Task.Delay(applicationSettings.BackgroundServiceTimeoutInterval,
                stoppingToken);
        }
    }
}
