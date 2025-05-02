using Microsoft.Extensions.DependencyInjection;
using Refit;
using TestTaskAlreadyMedia.Core.ExternalServices;

namespace TestTaskAlreadyMedia.Core;

public static class DependencyInjection
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        services.AddAutoMapper(assembly);
        services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));
    }

    public static void AddRefitServices(this IServiceCollection services)
    {
        var refitSettings = new RefitSettings(new NewtonsoftJsonContentSerializer());
        services.AddRefitClient<INasaDatasetApi>(refitSettings)
            .ConfigureHttpClient((provider, client) =>
            {
                client.BaseAddress = new Uri("https://raw.githubusercontent.com/biggiko/nasa-dataset");
            });
    }
}
