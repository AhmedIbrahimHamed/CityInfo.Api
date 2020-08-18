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

        [HttpGet("{pointId}", Name ="GetPointOfinterest")]
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

        [HttpPost]
        public IActionResult CreatePointOfInterest(int cityId,
            [FromBody] PointOfInterestForCreationDto pointOfInterest) {

            if (pointOfInterest.Name == pointOfInterest.Description) {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name."
                    );
            }

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            CityDto selectedCity = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (selectedCity == null) {
                return NotFound();
            }

            //demo purpose - to be improved
            var maxPointOfInterest = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

            var finalPointOfInterest = new PointOfInterestDto() {
                Id = ++maxPointOfInterest,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            selectedCity.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute(new { cityId, id = finalPointOfInterest.Id }, finalPointOfInterest);
        }

        [HttpPut("{pointId}")]
        public IActionResult UpdatePointOfInterest(int cityId, int pointId,
            [FromBody] PointOfInterestforUpdateDto pointOfInterestforUpdate) {

            if (pointOfInterestforUpdate.Name == pointOfInterestforUpdate.Description) {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name."
                    );
            }

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            CityDto selectedCity = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (selectedCity == null) {
                return NotFound();
            }

            PointOfInterestDto pointOfInterestFromStore = selectedCity.PointsOfInterest.FirstOrDefault(p => p.Id == pointId);

            if (pointOfInterestFromStore == null) {
                return NotFound();
            }

            pointOfInterestFromStore.Name = pointOfInterestforUpdate.Name;
            pointOfInterestFromStore.Description = pointOfInterestforUpdate.Description;

            return NoContent();

        }

        
    }
}
