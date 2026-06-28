using Cityinfo.API.Entities;
using System.Drawing;

namespace Cityinfo.API.Repository
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<City?> GetCityAsync(int CityId, bool includePointOfInterest);
        Task<bool> CityExistsAsync(int cityId);
        Task<IEnumerable<PointOfInterest>>
            GetPointsOfInterestForCityAsync(int CityId);
        Task<PointOfInterest?> GetPointsOfInterestForCityAsync(int CityId
            ,int PointOfInterestID);

        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
         void DeletePointOfInterest(PointOfInterest pointOfInterest);
        Task<bool> SaveChangesAsync();
    }
}
