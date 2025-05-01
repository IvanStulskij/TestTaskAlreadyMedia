using Hangfire;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TestTaskAlreadyMedia.Core.Models;

namespace TestTaskAlreadyMedia.Core.Jobs;

public class JobsHostedService : IHostedService
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IOptions<CommonSettings> _options;

    public JobsHostedService(IRecurringJobManager recurringJobManager, IOptions<CommonSettings> options)
    {
        _recurringJobManager = recurringJobManager;
        _options = options;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var options = _options.Value;

        _recurringJobManager.AddOrUpdate<GetNasaObjectsJob>(nameof(GetNasaObjectsJob), j => j.Process(), options.CheckNasaObjectsJobCronExpression);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}