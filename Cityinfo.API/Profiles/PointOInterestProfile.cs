using AutoMapper;

namespace Cityinfo.API.Profiles
{
    public class PointOInterestProfile : Profile
    {
        public PointOInterestProfile()
        {
            CreateMap<Entities.PointOfInterest,Models.PointOfInterestDto>();
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();
            CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
            CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>();
           

        }
    }
}
