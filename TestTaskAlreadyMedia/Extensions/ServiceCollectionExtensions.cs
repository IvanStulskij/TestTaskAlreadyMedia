using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Options;

namespace TestTaskAlreadyMedia.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddHangfire(this IServiceCollection services)
    {
        services.AddHangfire((provider, config) =>
        {
            var endpointOptions = provider.GetRequiredService<IOptions<DbSettings>>().Value;
            config.UsePostgreSqlStorage(endpointOptions.ConnectionString ?? throw new ArgumentException("PostgreSQL connection string is not specified."));
        });

        services.AddHangfireServer();
    }
}
