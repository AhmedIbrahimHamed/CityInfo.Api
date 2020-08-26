using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services {
    public interface ICityInfoRepository {

        IEnumerable<City> GetCities();

        City GetCity(int cityId, bool includePointsOfInterest);

        IEnumerable<PointOfInterest> GetPointsOfInterest(int cityId);

        PointOfInterest GetPointOfInterest(int cityId, int PointOfInterestId);

        bool CityExists(int cityId);

        void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);

        void UpdatedPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);

        void DeletePointOfInterestForCity(PointOfInterest pointOfInterest);

        bool Save();
    }
}
