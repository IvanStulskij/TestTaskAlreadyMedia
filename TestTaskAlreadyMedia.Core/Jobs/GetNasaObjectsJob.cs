using TestTaskAlreadyMedia.Core.ExternalServices;
using TestTaskAlreadyMedia.Core.Models;

namespace TestTaskAlreadyMedia.Core.Jobs;

public class GetNasaObjectsJob
{
    private readonly INasaDatasetApi _nasaApi;

    public GetNasaObjectsJob(INasaDatasetApi nasaApi)
    {
        _nasaApi = nasaApi;
    }

    public async Task Process()
    {
        
    }

    public async Task<IEnumerable<NasaObjectDto>> GetExternalNasaObjects()
    {
        var nasaObjects = await _nasaApi.GetNasaObjects();

        return nasaObjects;
    }
}
