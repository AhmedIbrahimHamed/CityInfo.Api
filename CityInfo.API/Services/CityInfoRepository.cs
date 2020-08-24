using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Contexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services {
    public class CityInfoRepository : ICityInfoRepository {

        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<City> GetCities() {
            return _context.Cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterest) {
            if (includePointsOfInterest) {
                return _context.Cities.Include(c => c.PointsOfInterest).Where(c => c.Id == cityId).FirstOrDefault();
            } else {
                return _context.Cities.Where(c => c.Id == cityId).FirstOrDefault();

            }
        }

        public PointOfInterest GetPointOfInterest(int cityId, int PointOfInterestId) {
            return _context.PointOfInterests.Where(p => p.CityId == cityId && p.Id == PointOfInterestId).FirstOrDefault();
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterest(int cityId) {
            return _context.PointOfInterests.Where(p => p.CityId == cityId ).ToList();

        }

        public bool CityExists(int cityId) {
            return _context.Cities.Any(c => c.Id == cityId);
        }

        public void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest) {
            City selectedCity = GetCity(cityId, false);

            selectedCity.PointsOfInterest.Add(pointOfInterest);
            
        }

        public void UpdatedPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest) {

        }

        public bool Save() {
            return (_context.SaveChanges() > 0);
        }
    }
}
