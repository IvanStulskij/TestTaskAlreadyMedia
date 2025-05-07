using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using TestTaskAlreadyMedia.Infrasructure.Models;

namespace TestTaskAlreadyMedia.Infrasructure;

public static class DependencyInjection
{
    public static void AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            var endpointOptions = provider.GetRequiredService<IOptions<DbSettings>>().Value;
            options.UseNpgsql(endpointOptions.ConnectionString ?? throw new ArgumentException("PostgreSQL connection string is not specified."));
            options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        });
    }

    public static async Task ApplyMigrations(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (dbContext.Database != null && dbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory") return;

        await dbContext.Database!.MigrateAsync();

        if (dbContext.Database!.GetDbConnection() is NpgsqlConnection npgsqlConnection)
        {
            await npgsqlConnection.OpenAsync();
            try
            {
                npgsqlConnection.ReloadTypes();
            }
            finally
            {
                await npgsqlConnection.CloseAsync();
            }
        }
    }
}
