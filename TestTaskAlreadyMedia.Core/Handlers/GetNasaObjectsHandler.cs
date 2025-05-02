using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TestTaskAlreadyMedia.Core.Models;
using TestTaskAlreadyMedia.Infrasructure;

namespace TestTaskAlreadyMedia.Core.Handlers;

public class GetNasaObjectsRequest : IRequest<LoadResult>
{
    public DataSourceLoadOptions DataSourceLoadOptions { get; set; }
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
        ValiateDataSourceLoadOptions(request.DataSourceLoadOptions);
        LoadResult nasaObjects;

        if (request.DataSourceLoadOptions.Group != null && request.DataSourceLoadOptions.Group.Any())
        {
            var getNasaObjectsQuery = from nasaObject in _context.NasaObjects.AsNoTracking()
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
            var grouppedData = getNasaObjectsQuery.GroupBy(x => x.Year.Year);
            request.DataSourceLoadOptions.Group = null;

            nasaObjects = await DataSourceLoader.LoadAsync(grouppedData, request.DataSourceLoadOptions, cancellationToken);
        }
        else
        {
            var getNasaObjectsQuery = from nasaObject in _context.NasaObjects.AsNoTracking()
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
            nasaObjects = await DataSourceLoader.LoadAsync(getNasaObjectsQuery, request.DataSourceLoadOptions, cancellationToken);
        }

        return nasaObjects;
    }

    private void ValiateDataSourceLoadOptions(DataSourceLoadOptions dataSourceLoadOptions)
    {
        var availableSortings = new[] { nameof(NasaObjectDto.Year), nameof(NasaObjectDto.Mass) };

        if (dataSourceLoadOptions.Sort != null && !dataSourceLoadOptions.Sort.Select(x => x.Selector).All(x => availableSortings.Contains(x)))
        {
            throw new ValidationException($"Only types {string.Join(',', availableSortings)} available for sorting");
        }

        var availableGrouppings = new[] { nameof(NasaObjectDto.Year) };

        if (dataSourceLoadOptions.Group != null && !dataSourceLoadOptions.Group.Select(x => x.Selector).All(x => availableGrouppings.Contains(x)))
        {
            throw new ValidationException($"Only types {string.Join(',', availableGrouppings)} available for groupping");
        }

        var availableFilterings = new[] { nameof(NasaObjectDto.Year), nameof(NasaObjectDto.Recclass), nameof(NasaObjectDto.Name) };

        if (dataSourceLoadOptions.Filter != null)
        {
            foreach (object exactFilter in dataSourceLoadOptions.Filter.Cast<object>())
            {
                if (exactFilter is IList<object>)
                {
                    var filter = dataSourceLoadOptions.Filter.Count > 0 ? dataSourceLoadOptions.Filter[0] as IList<object> : new List<object>();

                    if (!availableFilterings.Contains(filter.First().ToString()))
                    {
                        throw new ValidationException($"Only types {string.Join(',', availableFilterings)} available for filtering");
                    }
                }
            }
        }

        dataSourceLoadOptions.GroupSummary = null;
        dataSourceLoadOptions.TotalSummary = null;
    }
}
