using AutoMapper;
using TestTaskAlreadyMedia.Core.Models;
using TestTaskAlreadyMedia.Infrasructure.Models;

namespace TestTaskAlreadyMedia.Core;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<NasaObjectDto, NasaObject>()
            .ForMember(x => x.Coordinates, opt => opt.MapFrom(x => x.Geolocation == null ? null : x.Geolocation.Coordinates))
            .ForMember(x => x.GeolocationType, opt => opt.MapFrom(x => x.Geolocation == null ? null : x.Geolocation.Type))
            .ForMember(x => x.Year, opt => opt.MapFrom(x => x.Year.Year))
            .ForMember(x => x.NasaId, opt => opt.MapFrom(x => x.Id))
            .ForMember(x => x.Id, opt => opt.MapFrom(x => x.DbId == null ? Guid.Empty : x.DbId))
            .ReverseMap();
    }
}
