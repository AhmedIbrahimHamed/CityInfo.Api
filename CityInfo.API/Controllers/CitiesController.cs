using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers {
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase {
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper) {
            _cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetCities() {
            IEnumerable<City> cityEntities = _cityInfoRepository.GetCities();


            var results = _mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);
            //List<CityWithoutPointsOfInterestDto> results = new List<CityWithoutPointsOfInterestDto>();

            //foreach (City cityEntity in cityEntities) {
            //    results.Add(new CityWithoutPointsOfInterestDto() {
            //        Id = cityEntity.Id,
            //        Name = cityEntity.Name,
            //        Description = cityEntity.Description
            //    });
            //}


            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false) {

            City selectedCityEntity = _cityInfoRepository.GetCity(id, includePointsOfInterest);

            if (selectedCityEntity == null) {
                return NotFound();
            }

            if (includePointsOfInterest) {

                CityDto cityResult = _mapper.Map<CityDto>(selectedCityEntity);

                return Ok(cityResult);
                
            }

            CityWithoutPointsOfInterestDto cityWithoutPointsOfInterestResult = _mapper.Map<CityWithoutPointsOfInterestDto>(selectedCityEntity);

            return Ok(cityWithoutPointsOfInterestResult);

        }
    }
}
