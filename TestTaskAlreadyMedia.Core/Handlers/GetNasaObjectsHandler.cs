using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using MediatR;
using TestTaskAlreadyMedia.Core.Models;
using TestTaskAlreadyMedia.Infrasructure;

namespace TestTaskAlreadyMedia.Core.Handlers;

public class GetNasaObjectsRequest : IRequest<LoadResult>
{
    public DataSourceLoadOptionsBase DataSourceLoadOptions { get; set; }
}

public class GetNasaObjectsHandler : IRequestHandler<GetNasaObjectsRequest, LoadResult>
{
    private readonly ApplicationDbContext _context;

    public GetNasaObjectsHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<LoadResult> Handle(GetNasaObjectsRequest request, CancellationToken cancellationToken)
    {
        var getNasaObjectsQuery = from nasaObject in _context.NasaObjects
                                  select new NasaObjectDto
                                  {
                                      DbId = nasaObject.Id,
                                      Id = nasaObject.NasaId,
                                      Fall = nasaObject.Fall,
                                      Geolocation = nasaObject.Coordinates == null || nasaObject.GeolocationType == null ? null : new GeolocationDto
                                      {
                                          Coordinates = nasaObject.Coordinates,
                                          Type = nasaObject.GeolocationType,
                                      },
                                      Mass = nasaObject.Mass,
                                      Name = nasaObject.Name,
                                      NameType = nasaObject.NameType,
                                      Recclass = nasaObject.Recclass,
                                      Reclat = nasaObject.Reclat,
                                      Reclong = nasaObject.Reclong,
                                      Year = new DateTime(nasaObject.Year, 1, 1)
                                  };
        var nasaObjects = await DataSourceLoader.LoadAsync(getNasaObjectsQuery, request.DataSourceLoadOptions, cancellationToken);

        return nasaObjects;
    }
}
