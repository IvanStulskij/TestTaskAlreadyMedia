using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Options;
using TestTaskAlreadyMedia.Core.Models;
using TestTaskAlreadyMedia.Infrasructure.Models;

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

    public static void ConfigureOptions(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<DbSettings>(configuration.GetSection("DbSettings"));
        services.Configure<CommonSettings>(configuration.GetSection("CommonSettings"));
    }
}
