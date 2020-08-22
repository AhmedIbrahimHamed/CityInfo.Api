using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/testdatabase")]
    [ApiController]
    public class DummyController : ControllerBase
    {

        private readonly CityInfoContext _context;

        public DummyController(CityInfoContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        public IActionResult TestDatabase() {
            return Ok();
        }
    }
}