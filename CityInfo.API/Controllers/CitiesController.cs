using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers {
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase {

        [HttpGet]
        public IActionResult GetCities() {
            return Ok(CitiesDataStore.Current.Cities);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id) {
            CityDto selectedCity = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);

            if (selectedCity == null) {
                return NotFound();
            }

            return Ok(selectedCity);

        }
    }
}
