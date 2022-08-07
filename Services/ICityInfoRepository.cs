using CityInfoAPI.Entities;

namespace CityInfoAPI.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
        Task<IEnumerable<City>> GetCitiesAsync(string? name, string? searchQuery);
        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);
        Task<IEnumerable<PointOfInterest>> GetPointsOfInterestsForCityAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
        Task<bool> CityExistsAsync(int cityId);
        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
        Task<bool> SaveChangesAsync();
    }
}