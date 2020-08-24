using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controllers {
    [ApiController]
    [Route("api/cities/{cityId}/PointsOfInterest")]
    public class PointsOfInterestController : ControllerBase {

        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
                                          IMailService mailService,
                                          ICityInfoRepository cityInfoRepository,
                                          IMapper mapper) {

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId) {

            if (!_cityInfoRepository.CityExists(cityId)) {
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                return NotFound();
            }

            IEnumerable<PointOfInterest> selectedCityPointsEntity = _cityInfoRepository.GetPointsOfInterest(cityId);

            IEnumerable<PointOfInterestDto> pointsOfInterestsResult = _mapper.Map<IEnumerable<PointOfInterestDto>>(selectedCityPointsEntity);
            
            return Ok(pointsOfInterestsResult);
            
        }

        [HttpGet("{pointId}")]
        public IActionResult GetPointOfInterest(int cityId, int pointId) {

            if (!_cityInfoRepository.CityExists(cityId)) {
                return NotFound();
            }

            PointOfInterest pointOfInterestEntity = _cityInfoRepository.GetPointOfInterest(cityId, pointId);

            if (pointOfInterestEntity == null) {
                return NotFound();
            }

            PointOfInterestDto pointResult = _mapper.Map<PointOfInterestDto>(pointOfInterestEntity);

            return Ok(pointResult);
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


            if (!_cityInfoRepository.CityExists(cityId)) {
                return NotFound();
            }


            PointOfInterest finalPointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterest);

            _cityInfoRepository.AddPointOfInterestForCity(cityId, finalPointOfInterest);

            _cityInfoRepository.Save();

            PointOfInterestDto pointOfInterestDtoReturned = _mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute(new { cityId, id = pointOfInterestDtoReturned.Id }, pointOfInterestDtoReturned);
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


            if (!_cityInfoRepository.CityExists(cityId)) {
                return NotFound();
            }

            PointOfInterest pointOfInterestEntity = _cityInfoRepository.GetPointOfInterest(cityId, pointId);

            if (pointOfInterestEntity == null) {
                return NotFound();
            }

            _mapper.Map(pointOfInterestforUpdate, pointOfInterestEntity);

            _cityInfoRepository.UpdatedPointOfInterestForCity(cityId, pointOfInterestEntity);

            _cityInfoRepository.Save();


            return NoContent();

        }

        [HttpPatch("{pointId}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int pointId, 
            [FromBody] JsonPatchDocument<PointOfInterestforUpdateDto> patchDoc) {


            if (!_cityInfoRepository.CityExists(cityId)) {
                return NotFound();
            }

            PointOfInterest pointOfInterestFromStore = _cityInfoRepository.GetPointOfInterest(cityId, pointId);

            if (pointOfInterestFromStore == null) {
                return NotFound();
            }

            PointOfInterestforUpdateDto pointOfInterestToPatch = new PointOfInterestforUpdateDto() {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Name == pointOfInterestToPatch.Description) {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name."
                    );
            }

            if (!TryValidateModel(pointOfInterestToPatch)) {
                return BadRequest(ModelState);
            }

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{pointId}")]
        public IActionResult DeletePointOfInterest(int cityId, int pointId) {

            CityDto selectedCity = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (selectedCity == null) {
                return NotFound();
            }

            PointOfInterestDto pointOfInterestFromStore = selectedCity.PointsOfInterest.FirstOrDefault(p => p.Id == pointId);

            if (pointOfInterestFromStore == null) {
                return NotFound();
            }

            selectedCity.PointsOfInterest.Remove(pointOfInterestFromStore);

            _mailService.Send("Point of interest deleted",
                             $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted");

            return NoContent();

        }

        
    }
}
