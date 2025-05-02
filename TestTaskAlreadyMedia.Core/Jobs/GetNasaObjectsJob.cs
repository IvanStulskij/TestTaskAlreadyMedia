using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Refit;
using TestTaskAlreadyMedia.Core.ExternalServices;
using TestTaskAlreadyMedia.Core.Models;
using TestTaskAlreadyMedia.Infrasructure;
using TestTaskAlreadyMedia.Infrasructure.Models;

namespace TestTaskAlreadyMedia.Core.Jobs;

public class GetNasaObjectsJob
{
    private const string DuplicateNasaObject = "Selected NASA object already exist in database";
    private const int DuplicateNasaObjectErrorCode = 23505;

    private readonly INasaDatasetApi _nasaApi;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetNasaObjectsJob(INasaDatasetApi nasaApi, ApplicationDbContext context, IMapper mapper)
    {
        _nasaApi = nasaApi;
        _context = context;
        _mapper = mapper;
    }

    public async Task Process()
    {
        var externalNasaObjects = await GetExternalNasaObjects();
        var externalNasaObjectIds = externalNasaObjects.Select(x => x.Id);

        var dbNasaObjectsQuery = _context.NasaObjects.AsNoTracking()
            .Where(x => externalNasaObjectIds.Contains(x.NasaId));
        var dbNasaObjectNasaIds = await dbNasaObjectsQuery.Select(x => x.NasaId).ToListAsync();
        var dbNasaObjects = await dbNasaObjectsQuery.ToListAsync();

        var nasaObjectsToAdd = _mapper.Map<IEnumerable<NasaObject>>(externalNasaObjects.Where(x => !dbNasaObjectNasaIds.Contains(x.Id)));
        var nasaObjectIdsToDelete = await _context.NasaObjects.AsNoTracking()
            .Where(x => !externalNasaObjectIds.Contains(x.NasaId))
            .Select(x => x.Id)
            .ToListAsync();
        var nasaObjectsToUpdate = new List<NasaObject>();
        foreach (var externalNasaObject in externalNasaObjects)
        {
            var dbNasaObject = dbNasaObjects.FirstOrDefault(x => x.NasaId == externalNasaObject.Id);
            if (dbNasaObject != null && !CompareExternalNasaObjectToNasaObject(externalNasaObject, dbNasaObject))
            {
                var dbId = dbNasaObject.Id;
                dbNasaObject = _mapper.Map<NasaObject>(externalNasaObject);
                dbNasaObject.Id = dbId;
                nasaObjectsToUpdate.Add(dbNasaObject);
            }
        }

        var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            if (nasaObjectIdsToDelete.Any())
            {
                await _context.NasaObjects
                .Where(x => nasaObjectIdsToDelete.Contains(x.Id))
                .ExecuteDeleteAsync();
            }
            if (nasaObjectsToAdd.Any())
            {
                await _context.NasaObjects.AddRangeAsync(nasaObjectsToAdd);
            }
            if (nasaObjectsToUpdate.Any())
            {
                _context.NasaObjects.UpdateRange(nasaObjectsToUpdate);
            }
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (DbUpdateException exception)
        {
            await transaction.RollbackAsync();
            var message = exception.InnerException?.Message != null ? exception.InnerException.Message : exception.Message;

            if (message.Contains(DuplicateNasaObjectErrorCode.ToString()))
            {
                throw new ValidationException(DuplicateNasaObject);
            }

            throw;
        }
    }

    private async Task<IEnumerable<NasaObjectDto>> GetExternalNasaObjects()
    {
        IEnumerable<NasaObjectDto> nasaObjects;
        try
        {
            nasaObjects = await _nasaApi.GetNasaObjects();
        }
        catch (ApiException exception)
        {
            var message = exception.InnerException?.Message != null ? exception.InnerException.Message : exception.Message;

            throw new ValidationException($"Exception while getting data from resource : ${message}");
        }
        

        return nasaObjects;
    }

    private bool CompareExternalNasaObjectToNasaObject(NasaObjectDto externalObject, NasaObject nasaObject)
    {
        return externalObject.Mass == nasaObject.Mass && externalObject.Recclass == nasaObject.Recclass && externalObject.Fall == nasaObject.Fall
            && externalObject.Name == nasaObject.Name && externalObject.NameType == nasaObject.NameType && externalObject.Reclat == nasaObject.Reclat
            && externalObject.Reclong == nasaObject.Reclong && externalObject.Year.Year == nasaObject.Year && externalObject?.Geolocation?.Type == nasaObject.GeolocationType
            && (externalObject?.Geolocation == null || externalObject.Geolocation.Coordinates.SequenceEqual(nasaObject.Coordinates == null ? Enumerable.Empty<decimal>() : nasaObject.Coordinates));
    }
}