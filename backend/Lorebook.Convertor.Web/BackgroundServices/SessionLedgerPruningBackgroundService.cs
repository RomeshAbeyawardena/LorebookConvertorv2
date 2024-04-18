using Lorebook.Convertor.Domain;
using Microsoft.Extensions.Caching.Distributed;

namespace Lorebook.Convertor.Web.BackgroundServices;

public class SessionLedgerPruningBackgroundService(ISessionLedger sessionLedger, 
    IDistributedCache distributedCache) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if(await sessionLedger.HasExpiredEntries(stoppingToken))
            {
                await sessionLedger.RemoveExpired(distributedCache, stoppingToken);
            }

            await Task.Delay(10000, stoppingToken);
        }
    }
}
