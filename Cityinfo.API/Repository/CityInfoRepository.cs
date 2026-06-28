using Cityinfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cityinfo.API.Repository
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityinfoDbContex _Context;
        public CityInfoRepository(CityinfoDbContex contex)
        {
            _Context = contex ?? throw new ArithmeticException(nameof(contex));
        }
        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _Context.Cities.AnyAsync(c=>c.Id==cityId);
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _Context.Cities
                 .OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<City?> GetCityAsync(int CityId,bool includePointOfInterest)
        {
         if(includePointOfInterest)
            {
                return await _Context.Cities.Include(c => c.PointOfInterests)
                   .Where(c => c.Id == CityId).FirstOrDefaultAsync();
            }
            return await _Context.Cities
                   .Where(c => c.Id == CityId).FirstOrDefaultAsync();
        }

        public async Task<PointOfInterest?> GetPointsOfInterestForCityAsync(int CityId, int PointOfInterestID)
        {
            return await _Context.PointsOfInterests
                  .Where(p => p.CityId == CityId && p.Id == PointOfInterestID)
                  .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int CityId)
        {
            return await _Context.PointsOfInterests
                .Where(p => p.CityId == CityId)
                .ToListAsync();
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var city = await GetCityAsync(cityId, false);
            if(city != null)
            {
                city.PointOfInterests.Add(pointOfInterest);
            }

        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _Context.SaveChangesAsync() >0);
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _Context.PointsOfInterests.Remove(pointOfInterest);
        }
    }
}
