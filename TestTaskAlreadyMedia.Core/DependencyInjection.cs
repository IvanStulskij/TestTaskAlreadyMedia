using Microsoft.Extensions.DependencyInjection;
using Refit;
using TestTaskAlreadyMedia.Core.ExternalServices;
using Polly;
using Microsoft.Extensions.Options;
using TestTaskAlreadyMedia.Core.Models;

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
            })
            .AddPolicyHandler((provider, request) =>
            {
                var endPointOptions = provider.GetRequiredService<IOptions<CommonSettings>>().Value;
                var sleepDurations = new List<TimeSpan>();

                for (int i = 0; i < endPointOptions.NasaObjectsRetriesDelaysInSeconds.Length; i++)
                {
                    sleepDurations.Add(TimeSpan.FromSeconds(endPointOptions.NasaObjectsRetriesDelaysInSeconds[i]));
                }

                return Policy<HttpResponseMessage>.HandleResult(result => !result.IsSuccessStatusCode && !result.Content.ReadAsStringAsync().Result.Contains("уже авторизован"))
                        .OrInner<ApiException>()
                        .WaitAndRetryAsync(sleepDurations);
            }); ;
    }
}
