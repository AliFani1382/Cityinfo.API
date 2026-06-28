using AutoMapper;

namespace Cityinfo.API.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<Entities.City, Models.CityWithoutPointOfInterestDto>();
            CreateMap<Entities.City, Models.CityDto>()
                .ForMember(
                dest => dest.NumberOfPointsOfInterest,
                opt => opt.MapFrom(src => src.PointOfInterests.Count))
                .ForMember(
                dest => dest.PointOfInterest,
                opt => opt.MapFrom(src => src.PointOfInterests));


        }


    }
}
