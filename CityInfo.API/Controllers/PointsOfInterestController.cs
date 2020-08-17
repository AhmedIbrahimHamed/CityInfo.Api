using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers {
    [ApiController]
    [Route("api/cities/{cityId}/PointsOfInterest")]
    public class PointsOfInterestController : ControllerBase {

        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId) {
            CityDto selectedCity = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (selectedCity == null) {
                return NotFound();
            }

            return Ok(selectedCity.PointsOfInterest);

            
        }

        [HttpGet("{pointId}")]
        public IActionResult GetPointOfInterest(int cityId, int pointId) {
            CityDto selectedCity = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (selectedCity == null) {
                return NotFound();
            }

            PointOfInterestDto pointOfInterest = selectedCity.PointsOfInterest.FirstOrDefault(p => p.Id == pointId);

            if (pointOfInterest == null) {
                return NotFound();
            }

            return Ok(pointOfInterest);
        }

        
    }
}
