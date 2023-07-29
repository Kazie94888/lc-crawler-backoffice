using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using LC.Crawler.BackOffice.Configs;
using LC.Crawler.BackOffice.Extensions;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundWorkers.Hangfire;

namespace LC.Crawler.BackOffice.BackgroundWorkers.CleanData;

public class ClearAuditLogBackgroundWorker : HangfireBackgroundWorkerBase
{
    private readonly IAuditLogRepository _auditLogRepository;

    public ClearAuditLogBackgroundWorker(IAuditLogRepository auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
        RecurringJobId            = "ClearAuditLog_BackgroundWorker";
        CronExpression            = Cron.Daily(GlobalConfig.Crawler.SyncTimeHours,0);
    }

    public override async Task DoWorkAsync()
    {
        await CleanUpData();
    }
    
    private async Task CleanUpData()
    {
        var toDateTime = DateTime.UtcNow;
        var auditLogsKeepDays = 1;

        var oldAuditLogs =
            (await _auditLogRepository.GetListAsync(x => x.ExecutionTime < toDateTime.AddDays(-auditLogsKeepDays)))
            .ToList();
        foreach (var batch in oldAuditLogs.Partition(1000))
        {
            await _auditLogRepository.DeleteManyAsync(batch);
        }
    }
}